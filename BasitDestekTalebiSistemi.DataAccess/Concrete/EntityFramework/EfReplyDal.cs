using BasitDestekTalebiSistemi.Core.DataAccess.EntityFramework;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.Entities.Concrete;

namespace BasitDestekTalebiSistemi.DataAccess.Concrete.EntityFramework
{

    public class EfReplyDal : EfEntityRepositoryBase<Reply, AppDbContext>, IReplyDal
    {
    }
}
