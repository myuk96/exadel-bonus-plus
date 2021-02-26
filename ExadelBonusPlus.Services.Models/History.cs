using System;
using System.Collections.Generic;
using System.Text;

namespace ExadelBonusPlus.Services.Models
{
    public class History: IEntity<Guid>
    {
        public History()
        {
           
        }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ModifierId { get; set; }
        public bool IsDeleted { get; set; }
        public int Rating { get; set; }
        public Guid BonusId { get; set; }

    }
   public class HistoryComparer : IEqualityComparer<History>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(History x, History y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.BonusId == y.BonusId;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(History history)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(history, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hash = history.BonusId == null ? 0 : history.BonusId.GetHashCode();


            //Calculate the hash code for the product.
            return hash ;
        }
    }
}
