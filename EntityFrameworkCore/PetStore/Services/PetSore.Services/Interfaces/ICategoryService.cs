namespace PetSore.Services.Interfaces
{
    using System.Collections.Generic;
    using PetStore.Services.Model.Category;

    public interface ICategoryService
    {
        void Create(string name);

        IEnumerable<Categories> SearchCategoryByName(string name);
    }
}