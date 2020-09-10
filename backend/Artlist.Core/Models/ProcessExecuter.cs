using System;
using System.Collections.Generic;
using System.Text;

namespace Artlist.Core.Models.ProcessTasks
{

    public interface IProcessExecutorRequest
    {
        void Execute();
    }

    public class ProcessExecutorRequest<T> : IProcessExecutorRequest
    {
        private T _param;
        private Action<T> _func;

        public ProcessExecutorRequest(T param,Action<T> func)
        {
            _param = param;
            _func = func;
        }
       
        void IProcessExecutorRequest.Execute() {
            _func(this._param);
        }

      
    }
}
