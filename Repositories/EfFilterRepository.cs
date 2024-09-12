using DataAccessLayer.Concrete;
using EntityLayer;

namespace CustomsTracking_.Repositories
{
    public class EfFilterRepository:IFilterRepository
    {
        private Context _context;
        public EfFilterRepository(Context context)
        {
            _context = context;
        }
       
    }
}
