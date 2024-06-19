namespace NultienTest.Util;

public static class Util
{

    public static string ConstructQueryParams(Dictionary<string, string> queryParams)
    {
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

        foreach (string key in queryParams.Keys)
        {
            query[key] = queryParams[key];
        }

        return query.ToString();
    }
}