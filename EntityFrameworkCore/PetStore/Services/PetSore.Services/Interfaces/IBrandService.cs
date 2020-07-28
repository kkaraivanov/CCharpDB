namespace PetSore.Services.Interfaces
{
    using System.Collections.Generic;
    using PetStore.Services.Model.Brands;

    public interface IBrandService : IPetStore
    {
        void Create(string name);

        IEnumerable<Brands> SearchBrandByName(string name);
    }
}