
namespace BlazingPizza;

public class OrderState
{
    private readonly OrderStore orderStore;

    public OrderState(OrderStore orderStore)
    {
        this.orderStore = orderStore;
    }

    public bool ShowingConfigureDialog { get; private set; }

    public Pizza ConfiguringPizza { get; private set; }

    public Order Order { get; private set; } = new Order();

    public IReadOnlyList<Order> CompletedOrders =>
        orderStore.GetAllOrders();

    public void ShowConfigurePizzaDialog(PizzaSpecial special)
    {
        ConfiguringPizza = new Pizza
        {
            Special = special,
            SpecialId = special.Id,
            Size = Pizza.DefaultSize,
            Toppings = new List<PizzaTopping>()
        };

        ShowingConfigureDialog = true;
    }

    public void CancelConfigurePizzaDialog()
    {
        ConfiguringPizza = null;
        ShowingConfigureDialog = false;
    }

    public void ConfirmConfigurePizzaDialog()
    {
        Order.Pizzas.Add(ConfiguringPizza);

        ConfiguringPizza = null;
        ShowingConfigureDialog = false;
    }

    public void RemoveConfiguredPizza(Pizza pizza)
    {
        Order.Pizzas.Remove(pizza);
    }

    public int PlaceOrder()
    {
        if (!Order.Pizzas.Any())
        {
            throw new InvalidOperationException(
                "No pizzas selected.");
        }

        Order.CreatedTime = DateTime.Now;

        int orderId = orderStore.SaveOrder(Order);

        ResetOrder();

        return orderId;
    }

    public void ResetOrder()
    {
        Order = new Order();
        ConfiguringPizza = null;
        ShowingConfigureDialog = false;
    }
}
