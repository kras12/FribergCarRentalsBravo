namespace FribergCarRentalsBravo.DataAccess.Entities
{
    public class CarCategory
    {
        #region Constructors

        public CarCategory(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public int CarCategoryId { get; set; }

        public string Name { get; set; } = "";

        #endregion
    }
}
