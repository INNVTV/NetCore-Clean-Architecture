using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Policies
{
    /// <summary>
    /// Used as an example. From Chapter 1 of DDD by Eric Evans
    /// When it is important that certain business polices have artifacts
    /// That need to be shared and easily referenced by non-technical domain experts
    /// It is best to segment these as in a layer within the Domain library.
    /// This way important policy logic is not burried within a method
    /// </summary>
    public class OverbookingPolicy
    {

        // This enforces an "overbooking policy" which allows for an over booking of cargo at 110% of capacity
        // It will be called by the method that books cargo onto frieght.
        // You can share this with domain experts as it is more readable than code buried in a complex method
       
        /*
        public bool isAllowed(Cargo cargo, Voyage voyage)
        {
            return (cargo.size() + voyage.bookedCargoSize())  <= (voyage.capacity * 1.1);
        }
        */
    }
}
