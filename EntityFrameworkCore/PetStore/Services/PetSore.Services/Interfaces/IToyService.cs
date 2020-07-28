namespace PetSore.Services.Interfaces
{
    using System.Collections.Generic;
    using PetStore.Services.Model.Toy;

    public interface IToyService
    {
        void Create(string name, string description, decimal price, string brandName, string categoryName);

        IEnumerable<Toys> SearchToysByName(string name);

        IEnumerable<Toys> ListToysByCategory(string categoryName);
    }
}