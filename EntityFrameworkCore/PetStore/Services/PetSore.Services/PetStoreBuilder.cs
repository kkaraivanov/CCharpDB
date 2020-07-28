namespace PetSore.Services
{
    using Interfaces;
    using PetStore.Data;

    public class PetStoreBuilder : IPetStore
    {
        private readonly PetStoreDbContext db;

        public PetStoreBuilder(PetStoreDbContext context) => db = context;

    }
}