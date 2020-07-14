namespace P03_SalesDatabase.Data.Managment.Contracts
{
    public interface IWriter
    {
        void Write(string str);

        void WriteLine(string str);
    }
}