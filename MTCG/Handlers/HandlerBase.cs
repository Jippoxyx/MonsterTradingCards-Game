using MTCG.Http;
namespace MTCG.Handlers
{
    public abstract class HandlerBase<T> 
    {
        public HandlerBase(T parameters) { }

        public virtual HttpResponse POST()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.bad;
            return res;
        }

        public virtual HttpResponse GET()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.bad;
            return res;
        }
        public virtual HttpResponse PUT()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.bad;
            return res;
        }

        public virtual HttpResponse DELETE()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.bad;
            return res;
        }
    }
}