using MTCG.Http;
namespace MTCG.Handlers
{
    abstract class ControllerBase<T> 
    {
        public ControllerBase(T parameters) { }

        HttpResponse res = new HttpResponse();
        

        public virtual HttpResponse POST() {
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
          
        public virtual HttpResponse GET()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
        public virtual HttpResponse PUT()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }

        public virtual HttpResponse DELETE()
        {
            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
    }
}