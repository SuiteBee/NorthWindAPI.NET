using AutoMapper;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Views.Interfaces;

namespace NorthWindAPI.Views
{
    public class DashboardView(
        IOrderService orderService,
        IProductService productService,
        ICustomerService customerService,
        IMapper mapper, ILogger<DashboardView> logger
    ) : IDashboardView
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IProductService _productService = productService;
        private readonly ICustomerService _customerService = customerService;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DashboardView> _logger = logger;

        private readonly string dateFormat = "yyyy-MM-dd";

        #region " Data Sets "

        #region " Totals "

        public async Task<TotalsDto> GetTotals()
        {
            var orderList = await _orderService.ListOrders();
            var productList = await _productService.ListProducts();
            var clientList = await _customerService.ListCustomers();

            var orderTotals = new TotalsDto
            {
                NumOrders = orderList.Count(),
                RevenueTotal = orderList.Sum(o => o.OrderSubtotal),
                NumClients = clientList.Count(),
                NumProducts = productList.Count(p => !p.Discontinued),
                ProductsSold = orderList.SelectMany(o => o.Items).Sum(i => i.Quantity),
                Countries = orderList.Select(o => o.SendTo.Address.Country).Distinct().Count()
            };

            return orderTotals;
        }

        #endregion

        #region " Revenue Total "

        public async Task<IEnumerable<RevenueDto>> RevenueTotals()
        {
            var orderList = await _orderService.ListOrders();

            var totalsByYear = new List<RevenueDto>();
            var ordersByYear = orderList.GroupBy(o => o.OrderDate.Substring(0, 4));

            foreach (var orderYear in ordersByYear)
            {
                var currentTotalDto = CalculateYearTotals(orderYear);
                CalculateQuarterTotals(currentTotalDto, orderYear);

                totalsByYear.Add(currentTotalDto);
            }

            return totalsByYear;
        }

        private static RevenueDto CalculateYearTotals(IGrouping<string, OrderDto> orderYear)
        {
            var currentYear = orderYear.Key;
            var currentYearTotal = orderYear.Sum(o => o.OrderTotal);

            var currentTotalDto = new RevenueDto { Year = currentYear, Total = currentYearTotal };
            return currentTotalDto;
        }

        private static void CalculateQuarterTotals(RevenueDto currentTotalDto, IGrouping<string, OrderDto> orderYear)
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
            var orderList = await _orderService.ListOrders();
            var itemList = new List<OrderItemDto>();

            foreach (OrderDto order in orderList)
            {
                itemList.AddRange(order.Items);
            }

            var allTotal = itemList.Sum(i => i.FinalPrice);
            var totalsByCategory = new List<CategoryRatiosDto>();
            var ordersByCategory = itemList.GroupBy(i => i.CategoryName);

            foreach (var orderCat in ordersByCategory)
            {
                var currentCategory = CalculateCategoryTotal(allTotal, orderCat);
                totalsByCategory.Add(currentCategory);
            }

            return totalsByCategory;
        }

        private static CategoryRatiosDto CalculateCategoryTotal(decimal allTotal, IGrouping<string, OrderItemDto> items)
        {
            var catTotal = items.Sum(c => c.FinalPrice);
            var catPercentage = Math.Round(decimal.Divide(catTotal, allTotal) * 100);
            return new CategoryRatiosDto() { Category = items.Key, Percentage = catPercentage };
        }

        #endregion

        #region " Category Revenue " 

        public async Task<IEnumerable<CategoryRevenueDto>> CategoryRevenue()
        {
            var orderList = await _orderService.ListOrders();

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
            for (int i = minYear; i <= maxYear; i++)
            {
                if (i > minYear)
                {
                    var currentYear = categoriesByYear.FirstOrDefault(c => c.Year == i);
                    var previousYear = categoriesByYear.First(c => c.Year == i - 1);

                    //Fill in empty years with previous year total
                    if (currentYear == null)
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

        private static CategoryRevenueDto CalculateCategoryYear(IGrouping<int, OrderDto> orderYear)
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
            var orderList = await _orderService.ListOrders();
            return orderList.Count(o => o.SendTo.ShippedDate == null);
        }

        #endregion

        #region " Category Heatmap "

        public async Task<IEnumerable<CategoryHeatmapDto>> CategoryHeatmap()
        {
            var orderList = await _orderService.ListOrders();

            var categoriesByMonth = new List<CategoryHeatmapDto>();
            var ordersByMonth = orderList.GroupBy(o => int.Parse(o.OrderDate.Substring(5, 2)));

            foreach (var orderMonth in ordersByMonth)
            {
                var categoryMonth = CountCategoryItemsMonth(orderMonth);
                categoriesByMonth.Add(categoryMonth);
            }

            return categoriesByMonth.OrderBy(x => x.Month);
        }

        private static CategoryHeatmapDto CountCategoryItemsMonth(IGrouping<int, OrderDto> orderMonth)
        {
            var categoriesByMonth = orderMonth.SelectMany(o => o.Items).GroupBy(i => i.CategoryName);
            var currentMonthCategoryItems = new CategoryHeatmapDto() { Month = orderMonth.Key };

            foreach (var categoryMonth in categoriesByMonth)
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
