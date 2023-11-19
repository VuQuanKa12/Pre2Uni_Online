using JOT23_Pre2UniOnline.Models;

namespace JOT23_Pre2UniOnline.DatAccess
{
    public interface IUserDAO<t>
    {
        private static t instance;
        public static t Instance;
        public void GetAll();
        public bool Delete(int index);
        public bool Update(Profile obj);
        public void SaveChange();
        public bool AddNew(Profile obj);
    }
}
