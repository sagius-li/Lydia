using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Xml.Schema;
using System.Text;

using OCG.ResourceManagement.Client.WSClient;
using OCG.ResourceManagement.Client.WSFactory;
using OCG.ResourceManagement.Client.WSMessage;

using OCG.ResourceManagement.ObjectModel;

namespace OCG.ResourceManagement.Client.WSEnumeration
{
    public class ResultEnumerator : IEnumerator<RmResource>, IEnumerable<RmResource>
    {
        #region Private Members

        private WSEnumerationClient client;
        private RequestFactory requestFactory;
        private ResponseFactory responseFactory;
        private RmResourceFactory resourceFactory;

        private List<RmResource> results;
        private int resultIndex;
        private bool endOfSequence;
        private EnumerationContext context;
        private string filter;
        private string[] attributes;
        private RmResource current;

        #endregion

        #region Constructor

        public ResultEnumerator(WSEnumerationClient client, RequestFactory requestFactory, ResponseFactory responseFactory, 
            RmResourceFactory resourceFactory, string filter, string[] attributes)
        {
            this.results = new List<RmResource>();

            this.client = client;
            this.requestFactory = requestFactory;
            this.responseFactory = responseFactory;
            this.resourceFactory = resourceFactory;

            this.filter = filter;
            this.attributes = attributes;
        }

        #endregion

        #region IEnumerator Members

        public RmResource Current
        {
            get { return this.current; }
        }

        public void Dispose()
        {
            this.context = null;
            this.results.Clear();
            this.results = null;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.current; }
        }

        public bool MoveNext()
        {
            lock (this.client)
            {
                if (resultIndex < results.Count)
                {
                    this.current = results[resultIndex++];
                    return true;
                }
                else
                {
                    ResponsePull response;
                    if (this.context == null)
                    {
                        if (resultIndex > 0)
                        {
                            // case: previous pull returned an invalid context
                            return false;
                        }
                        RequestEnumeration request = new RequestEnumeration(filter);
                        if (attributes != null)
                        {
                            request.Selection = new List<string>();
                            request.Selection.AddRange(this.attributes);
                        }

                        Message msgRequest = requestFactory.CreateEnumerationRequest(request);
                        Message msgResponse = client.Enumerate(msgRequest);
                        if (msgResponse.IsFault)
                        {
                            ClientHelper.HandleFault(msgResponse);
                        }

                        response = responseFactory.CreateEnumerationResponse(msgResponse);
                        this.endOfSequence = response.EndOfSequence != null;
                    }
                    else
                    {
                        if (this.endOfSequence == true)
                        {
                            // case: previous pull returned an end of sequence flag
                            this.current = null;
                            return false;
                        }

                        RequestPull request = new RequestPull();
                        request.EnumerationContext = this.context;

                        Message msgRequest = requestFactory.CreatePullRequest(request);
                        Message msgResponse = client.Pull(msgRequest);
                        if (msgResponse.IsFault)
                        {
                            ClientHelper.HandleFault(msgResponse);
                        }

                        response = responseFactory.CreatePullResponse(msgResponse);
                    }

                    if (response == null)
                        return false;

                    resultIndex = 0;
                    this.results = resourceFactory.CreateResource(response, false).ConvertAll<RmResource>(x => x as RmResource);
                    this.context = response.EnumerationContext;
                    this.endOfSequence = response.IsEndOfSequence;

                    if (this.results.Count > 0)
                    {
                        this.current = results[resultIndex++];
                        return true;
                    }
                    else
                    {
                        this.current = null;
                        return false;
                    }
                }
            }
        }

        public void Reset()
        {
            this.results.Clear();
            this.context = null;
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator<RmResource> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion
    }
}
