namespace MealPlanner.Models.VM
{
    public class MealCreateViewModel
    {
        public string Name { get; set; }
        public int CookingTime { get; set; }

        public List<IngredientSelection> Ingredients { get; set; } = new();
    }

    public class IngredientSelection
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public int Quantity { get; set; }
    }

}
