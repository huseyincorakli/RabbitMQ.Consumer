using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

#region Bağlantı Oluşturma

ConnectionFactory factory = new();
factory.Uri = new("amqps://ymsclgok:bW2XZJGz5gmJSCs7D0NFtcX_RxbHvkR-@sparrow.rmq.cloudamqp.com/ymsclgok");

#endregion

#region Bağlantıyı Aktifleştirme ve Kanal Açma

using IConnection connection = factory.CreateConnection(); //Bağlantı Aktifleştirme
using IModel channel = connection.CreateModel(); //Kanal açma
#endregion

#region Queue Oluşturma 

channel.QueueDeclare(queue: "example-queue", exclusive: false,durable:true);

#endregion


#region Queuedan Gelen Mesajı Okuma 

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck:false, consumer); //mesajın onaylanma süreci için autoAck:false
channel.BasicQos(0, 5, false);
consumer.Received += (sender, e) =>
{

    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) + " işlendi");
    //consumer mesajı başarılı şekilde işlediğini burada belirtir.
    // delivery tag her mesaja ait   unique bir değeri temsil eder.
    //multiple parametresi ise; eğer değer false ise sadece bu tage sahip mesajın işlendiğini true ise bu tage sahip ve bundan önceki tüm mesakların işlenediğini bildirir.
    channel.BasicAck(e.DeliveryTag, multiple: false);
    
};
#endregion


Console.Read();