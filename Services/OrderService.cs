using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Controllers.Models.Requests;

namespace NorthWindAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        private readonly string dateFormat = "yyyy-MM-dd";

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;

            _mapper = mapper;
            _logger = logger;
        }

        #region " SELECT "

        public async Task<IEnumerable<OrderDto>> ListOrders()
        {
            var orderList = await _orderRepository.AllOrders();
            var detailList = await _orderRepository.AllDetails();
            var productList = await _productRepository.AllProducts();
            var categoryList = await _productRepository.AllCategories();
            var carrierList = await _orderRepository.AllCarriers();

            var orderDto = _mapper.Map<IEnumerable<OrderDto>>(orderList);

            foreach(OrderDto dto in orderDto)
            {
                var order = orderList.First(x => x.Id == dto.OrderId);
                _mapper.Map(order, dto.SendTo);
                _mapper.Map(order, dto.SendTo.Address);

                var detailsDto = detailList.Where(x => x.OrderId == dto.OrderId);
                foreach(OrderDetail detail in detailsDto)
                {
                    Product prod = productList.First(x => x.Id == detail.ProductId);
                    Category cat = categoryList.First(x => x.Id == prod.CategoryId);

                    OrderItemDto itemDto = _mapper.Map<OrderItemDto>(prod);
                    _mapper.Map(cat, itemDto);
                    _mapper.Map(detail, itemDto);

                    CalculateItemTotals(itemDto);

                    dto.Items.Add(itemDto);
                }

                var shippedBy = carrierList.First(x => x.Id == order.ShipVia);
                _mapper.Map(shippedBy, dto.SendTo);

                CalculateOrderTotal(dto);
            }

            return orderDto;
        }

        public async Task<OrderDto> FindOrder(int orderId)
        {
            var orderBase = await _orderRepository.FindOrder(orderId);
            var orderDetails = await _orderRepository.FindDetail(orderId);

            var orderDto = _mapper.Map<OrderDto>(orderBase);
            _mapper.Map(orderBase, orderDto.SendTo);
            _mapper.Map(orderBase, orderDto.SendTo.Address);

            foreach (OrderDetail detail in orderDetails)
            {
                Product prod = await _productRepository.FindProduct(detail.ProductId);
                Category cat = await _productRepository.FindCategory(prod.CategoryId);

                OrderItemDto itemDto = _mapper.Map<OrderItemDto>(prod);
                _mapper.Map(cat, itemDto);
                _mapper.Map(detail, itemDto);

                CalculateItemTotals(itemDto);

                orderDto.Items.Add(itemDto);
            };

            var shippedBy = await _orderRepository.FindCarrier(orderBase.ShipVia);
            _mapper.Map(shippedBy, orderDto.SendTo);

            CalculateOrderTotal(orderDto);

            return orderDto;

        }

        #endregion

        #region " INSERT "

        public async Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder)
        {
            var toInsert = _mapper.Map<Order>(newOrder);
            _mapper.Map(newOrder.Address, toInsert);

            var orderDate = DateTime.Today;
            var orderDateString = orderDate.ToString(dateFormat);
            toInsert.OrderDate = orderDateString;
            toInsert.RequiredDate = orderDate.AddDays(28).ToString(dateFormat);

            var inserted = await _orderRepository.InsertOrder(toInsert);

            foreach(var detail in newOrder.OrderDetail)
            {
                Product prod = await _productRepository.FindProduct(detail.ProductId);

                var toInsertDetail = _mapper.Map<OrderDetail>(detail);
                toInsertDetail.Id = $"{inserted.Id}/{detail.ProductId}";
                toInsertDetail.OrderId = inserted.Id;
                toInsertDetail.UnitPrice = prod.UnitPrice;

                await _orderRepository.InsertDetail(toInsertDetail);
            }

            return await FindOrder(inserted.Id);
        }

        #endregion

        #region " UPDATE "

        public async Task<OrderDto> MarkAsShipped(int orderId, string? shippedDate = null)
        {
            var orderBase = await _orderRepository.FindOrder(orderId);

            DateTime shipDate = shippedDate == null ? DateTime.Today : DateTime.Parse(shippedDate);
            var shipDateString = shipDate.ToString(dateFormat);
            orderBase.ShippedDate = shipDateString;

            await _orderRepository.UpdateOrder(orderId, orderBase);
            return await FindOrder(orderId);
        }

        public async Task<IEnumerable<OrderDto>> MarkAsShipped(ShipRequest orders)
        {
            var toReturn = new List<OrderDto>();

            foreach(int order in orders.OrderIds)
            {
                
                var shipped = await MarkAsShipped(order, orders.ShipDate);
                toReturn.Add(shipped);
            }

            return toReturn;
        }

        #endregion

        #region " DELETE "

        public async Task<bool> RemoveOrder(int orderId)
        {
            return await _orderRepository.DeleteOrder(orderId);
        }

        #endregion

        #region " Business Logic "

        private void CalculateOrderTotal(OrderDto dto)
        {
            dto.OrderSubtotal = dto.Items.Sum(x => x.FinalPrice);
            dto.OrderTotal = dto.OrderSubtotal + dto.SendTo.ShipCost;
        }

        private void CalculateItemTotals(OrderItemDto dto)
        {
            dto.TotalPrice = GetTotalPrice(dto);
            dto.DiscountAmt = GetDiscount(dto);
            dto.FinalPrice = GetFinalPrice(dto);
        }

        private decimal GetTotalPrice(OrderItemDto dto)
        {
            return Math.Round(dto.PurchasePrice * dto.Quantity, 2);
        }

        private decimal GetDiscount(OrderItemDto dto)
        {
            return dto.TotalPrice * (decimal)dto.Discount;
        }

        private decimal GetFinalPrice(OrderItemDto dto)
        {
            if(dto.Discount > 0)
            {
                decimal finalPrice = dto.TotalPrice * (decimal)(1 - dto.Discount);
                return Math.Round(finalPrice, 2);
            }
            else
            {
                return dto.TotalPrice;
            }
        }

        #endregion

        #region " Data Sets "

        #region " Revenue Total "

        public async Task<IEnumerable<RevenueDto>> RevenueTotals()
        {
            var orderList = await ListOrders();

            var totalsByYear = new List<RevenueDto>();
            var ordersByYear = orderList.GroupBy(o => o.OrderDate.Substring(0, 4));

            foreach(var orderYear in ordersByYear)
            {
                var currentTotalDto = CalculateYearTotals(orderYear);
                CalculateQuarterTotals(currentTotalDto, orderYear);

                totalsByYear.Add(currentTotalDto);
            }

            return totalsByYear;
        }

        private RevenueDto CalculateYearTotals(IGrouping<string, OrderDto> orderYear)
        {
            var currentYear = orderYear.Key;
            var currentYearTotal = orderYear.Sum(o => o.OrderTotal);

            var currentTotalDto = new RevenueDto { Year = currentYear, Total = currentYearTotal };
            return currentTotalDto;
        }

        private void CalculateQuarterTotals(RevenueDto currentTotalDto, IGrouping<string, OrderDto> orderYear)
        {
            var ordersByQuarter = orderYear.GroupBy(o => (int.Parse(o.OrderDate.Substring(5, 2)) - 1) / 3);

            currentTotalDto.QuarterOne = 0;
            currentTotalDto.QuarterTwo = 0;
            currentTotalDto.QuarterThree = 0;
            currentTotalDto.QuarterFour = 0;

            foreach (var orderQuarter in ordersByQuarter)
            {
                var currentQuarterTotal = orderQuarter.Sum(q => q.OrderTotal);

                switch (orderQuarter.Key + 1)
                {
                    case 1:
                        currentTotalDto.QuarterOne = currentQuarterTotal;
                        break;
                    case 2:
                        currentTotalDto.QuarterTwo = currentQuarterTotal;
                        break;
                    case 3:
                        currentTotalDto.QuarterThree = currentQuarterTotal;
                        break;
                    case 4:
                        currentTotalDto.QuarterFour = currentQuarterTotal;
                        break;
                }
            }
        }

        #endregion

        #region " Category Ratios "

        public async Task<IEnumerable<CategoryRatiosDto>> CategoryRatios()
        {
            var orderList = await ListOrders();
            var itemList = new List<OrderItemDto>();
            
            foreach(OrderDto order in orderList)
            {
                itemList.AddRange(order.Items);
            }

            var allTotal = itemList.Sum(i => i.FinalPrice);
            var totalsByCategory = new List<CategoryRatiosDto>();
            var ordersByCategory = itemList.GroupBy(i => i.CategoryName);

            foreach(var orderCat in ordersByCategory)
            {
                var currentCategory = CalculateCategoryTotal(allTotal, orderCat);
                totalsByCategory.Add(currentCategory);
            }

            return totalsByCategory;
        }

        private CategoryRatiosDto CalculateCategoryTotal(decimal allTotal, IGrouping<string, OrderItemDto> items)
        {
            var catTotal = items.Sum(c => c.FinalPrice);
            var catPercentage = Math.Round(decimal.Divide(catTotal, allTotal) * 100);
            return new CategoryRatiosDto() { Category = items.Key, Percentage = catPercentage };
        }

        #endregion

        #region " Category Revenue " 

        public async Task<IEnumerable<CategoryRevenueDto>> CategoryRevenue()
        {
            var orderList = await ListOrders();

            var categoriesByYear = new List<CategoryRevenueDto>();
            var ordersByYear = orderList.GroupBy(o => int.Parse(o.OrderDate.Substring(0, 4)));
            var minYear = ordersByYear.Min(o => o.Key);
            var maxYear = ordersByYear.Max(o => o.Key);

            //Calculate category totals per year for existing order years
            foreach (var orderYear in ordersByYear)
            {
                var categoryYear = CalculateCategoryYear(orderYear);
                categoriesByYear.Add(categoryYear);
            }

            //Loop all years and fill-in data for missing years
            for(int i=minYear; i<=maxYear; i++)
            {
                if(i > minYear)
                {
                    var currentYear = categoriesByYear.FirstOrDefault(c => c.Year == i);
                    var previousYear = categoriesByYear.First(c => c.Year == i - 1);

                    //Fill in empty years with previous year total
                    if(currentYear == null)
                    {
                        var emptyYear = new CategoryRevenueDto()
                        {
                            Year = i,
                            Beverages = previousYear.Beverages,
                            Condiments = previousYear.Condiments,
                            Confections = previousYear.Confections,
                            Dairy = previousYear.Dairy,
                            Grains = previousYear.Grains,
                            Meat = previousYear.Meat,
                            Produce = previousYear.Produce,
                            Seafood = previousYear.Seafood
                        };
                        categoriesByYear.Add(emptyYear);
                    }
                    //Return category totals as additive - current year is sum of all previous
                    else
                    {
                        currentYear.Beverages += previousYear.Beverages;
                        currentYear.Condiments += previousYear.Condiments;
                        currentYear.Confections += previousYear.Confections;
                        currentYear.Dairy += previousYear.Dairy;
                        currentYear.Grains += previousYear.Grains;
                        currentYear.Meat += previousYear.Meat;
                        currentYear.Produce += previousYear.Produce;
                        currentYear.Seafood += previousYear.Seafood;
                    }
                }
            }

            return categoriesByYear.OrderBy(x => x.Year);
        }

        private CategoryRevenueDto CalculateCategoryYear(IGrouping<int, OrderDto> orderYear)
        {
            var categoriesByYear = orderYear.SelectMany(o => o.Items).GroupBy(i => i.CategoryName);
            var currentYearCategoryTotal = new CategoryRevenueDto() { Year = orderYear.Key };

            foreach (var categoryYear in categoriesByYear)
            {
                var currentTotal = categoryYear.Sum(i => i.FinalPrice);

                switch (categoryYear.Key)
                {
                    case "Beverages":
                        currentYearCategoryTotal.Beverages = currentTotal;
                        break;
                    case "Condiments":
                        currentYearCategoryTotal.Condiments = currentTotal;
                        break;
                    case "Confections":
                        currentYearCategoryTotal.Confections = currentTotal;
                        break;
                    case "Dairy Products":
                        currentYearCategoryTotal.Dairy = currentTotal;
                        break;
                    case "Grains/Cereals":
                        currentYearCategoryTotal.Grains = currentTotal;
                        break;
                    case "Meat/Poultry":
                        currentYearCategoryTotal.Meat = currentTotal;
                        break;
                    case "Produce":
                        currentYearCategoryTotal.Produce = currentTotal;
                        break;
                    case "Seafood":
                        currentYearCategoryTotal.Seafood = currentTotal;
                        break;
                }
            }

            return currentYearCategoryTotal;
        }

        #endregion

        #region " Pending Shipments "

        public async Task<int> PendingShipments()
        {
            var orderList = await ListOrders();
            return orderList.Count(o => o.SendTo.ShippedDate == null);
        }

        #endregion

        #region " Category Heatmap "

        public async Task<IEnumerable<CategoryHeatmapDto>> CategoryHeatmap()
        {
            var orderList = await ListOrders();

            var categoriesByMonth = new List<CategoryHeatmapDto>();
            var ordersByMonth = orderList.GroupBy(o => int.Parse(o.OrderDate.Substring(5, 2)));

            foreach(var orderMonth in ordersByMonth)
            {
                var categoryMonth = CountCategoryItemsMonth(orderMonth);
                categoriesByMonth.Add(categoryMonth);
            }

            return categoriesByMonth.OrderBy(x => x.Month);
        }

        private CategoryHeatmapDto CountCategoryItemsMonth(IGrouping<int, OrderDto> orderMonth)
        {
            var categoriesByMonth = orderMonth.SelectMany(o => o.Items).GroupBy(i => i.CategoryName);
            var currentMonthCategoryItems = new CategoryHeatmapDto() { Month = orderMonth.Key };

            foreach(var categoryMonth in categoriesByMonth)
            {
                var numOrders = categoryMonth.Count();

                switch (categoryMonth.Key)
                {
                    case "Beverages":
                        currentMonthCategoryItems.Beverages = numOrders;
                        break;
                    case "Condiments":
                        currentMonthCategoryItems.Condiments = numOrders;
                        break;
                    case "Confections":
                        currentMonthCategoryItems.Confections = numOrders;
                        break;
                    case "Dairy Products":
                        currentMonthCategoryItems.Dairy = numOrders;
                        break;
                    case "Grains/Cereals":
                        currentMonthCategoryItems.Grains = numOrders;
                        break;
                    case "Meat/Poultry":
                        currentMonthCategoryItems.Meat = numOrders;
                        break;
                    case "Produce":
                        currentMonthCategoryItems.Produce = numOrders;
                        break;
                    case "Seafood":
                        currentMonthCategoryItems.Seafood = numOrders;
                        break;
                }
            }

            return currentMonthCategoryItems;
        }

        #endregion

        #endregion
    }
}
