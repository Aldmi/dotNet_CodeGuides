namespace WorkWithConfig.ConfigureOptions;


public class ApplicationOptions
{
    public MyDbOption MyDbOption { get; set; }
    public MessageBrokerOption MessageBrokerOption { get; set; }
}

public class MyDbOption
{
    public string Connection { get; set; }
    public Config Config { get; set; }
}

public class Config
{
    public string param1 { get; set; }
}

public class MessageBrokerOption
{
    public string Url { get; set; }
    public string TopicName { get; set; }
}









