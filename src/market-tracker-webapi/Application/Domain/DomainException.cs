namespace market_tracker_webapi.Application.Domain;

public class DomainException(string name) : Exception("Domain exception: " + name);