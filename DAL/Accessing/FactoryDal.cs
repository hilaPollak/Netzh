namespace DAL.Accessing
{
    public class FactoryDal
    {
        public static DAL.Interface.Idal GetDal()
        {
            return new DAL.Implementations.SQLDataBase();
        }
    }
}
