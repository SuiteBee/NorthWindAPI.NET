﻿using AutoMapper;
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

        public async Task<IEnumerable<TotalsDto>> RevenueTotals()
        {
            var orderList = await ListOrders();

            var totalsByYear = new List<TotalsDto>();
            var ordersByYear = orderList.GroupBy(o => o.OrderDate.Substring(0, 4));

            foreach(var orderYear in ordersByYear)
            {
                var currentTotalDto = CalculateYearTotals(orderYear);
                CalculateQuarterTotals(currentTotalDto, orderYear);

                totalsByYear.Add(currentTotalDto);
            }

            return totalsByYear;
        }

        private TotalsDto CalculateYearTotals(IGrouping<string, OrderDto> orders)
        {
            var currentYear = orders.Key;
            var currentYearTotal = orders.Sum(o => o.OrderTotal);

            var currentTotalDto = new TotalsDto { Year = currentYear, Total = currentYearTotal };
            return currentTotalDto;
        }

        private void CalculateQuarterTotals(TotalsDto currentTotalDto, IGrouping<string, OrderDto> orders)
        {
            var ordersByQuarter = orders.GroupBy(o => (int.Parse(o.OrderDate.Substring(5, 2)) - 1) / 3);

            currentTotalDto.QuarterOne = 0;
            currentTotalDto.QuarterTwo = 0;
            currentTotalDto.QuarterThree = 0;
            currentTotalDto.QuarterFour = 0;

            foreach (var orderQuarter in ordersByQuarter)
            {
                var currentQuarterTotal = Math.Round(orderQuarter.Sum(q => q.OrderTotal));

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

        public async Task<IEnumerable<CategoryTotalsDto>> CategoryTotals()
        {
            var orderList = await ListOrders();
            var itemList = new List<OrderItemDto>();
            
            foreach(OrderDto order in orderList)
            {
                itemList.AddRange(order.Items);
            }

            var allTotal = itemList.Sum(i => i.FinalPrice);
            var totalsByCategory = new List<CategoryTotalsDto>();
            var ordersByCategory = itemList.GroupBy(i => i.CategoryName);

            foreach(var orderCat in ordersByCategory)
            {
                var currentCategory = CalculateCategoryTotal(allTotal, orderCat);
                totalsByCategory.Add(currentCategory);
            }

            return totalsByCategory;
        }

        private CategoryTotalsDto CalculateCategoryTotal(decimal allTotal, IGrouping<string, OrderItemDto> items)
        {
            var catTotal = items.Sum(c => c.FinalPrice);
            var catPercentage = Math.Round(decimal.Divide(catTotal, allTotal) * 100);
            return new CategoryTotalsDto() { Category = items.Key, Percentage = catPercentage };
        }

        #endregion
    }
}
