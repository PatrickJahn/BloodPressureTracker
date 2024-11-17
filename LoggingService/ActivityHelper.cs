using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Monitoring;

public static class ActivityHelper
{
    public static HttpRequestMessage AddActivityInfoToHttpRequest(HttpRequestMessage httpRequestMessage, Activity activity)
    {
        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
        var propagator = new TraceContextPropagator();
        
        propagator.Inject(propagationContext, httpRequestMessage, (r, key, value) =>
        {
            r.Headers.Add(key,value);
        });
        
        return httpRequestMessage;
    }
    
    public static PropagationContext ExtractPropagationContextFromHttpRequest(HttpRequest httpRequest)
    {
      
        var propagator = new TraceContextPropagator();
        
        var parentContext = propagator.Extract(default, httpRequest, (r, key) =>
        {
            return new List<string>( new [] {r.Headers.ContainsKey(key) ? r.Headers[key].ToString() : string.Empty});
        });

        return parentContext;
    }
}