using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://ymsclgok:bW2XZJGz5gmJSCs7D0NFtcX_RxbHvkR-@sparrow.rmq.cloudamqp.com/ymsclgok");


using IConnection connection = factory.CreateConnection(); //Bağlantı Aktifleştirme
using IModel channel = connection.CreateModel(); //Kanal açma

//1.adım publisherda bulunan exchangein aynısnı oluşturduk.
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2.adım bir kuyruk oluşturduk
string queueName = channel.QueueDeclare().QueueName;

//3.adım publisher da routing key belirlediğimiz routingkey ile oluşturduğumuz kuyruğu bind ettik
channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine("Message:"+message);
};

Console.Read();