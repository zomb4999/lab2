using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Sockets;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;

namespace lab3
{
    public partial class Form1 : Form
    {
        private byte[] data = new byte[1024];
        private IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 6000);
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        private const int port = 5678;
        private string ip;
        public Form1()
        {
            InitializeComponent();
        }

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();
        }

        private void AcceptClients()
        {
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Новое подключение");

                //Новый поток для обработки клиента
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Получено: " + message);

                    //Сообщение возвращается клиенту
                    byte[] response = Encoding.UTF8.GetBytes("Echo: " + message);
                    stream.Write(response, 0, response.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка соединения с клиентом: " + ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
                clients.Remove(client);
                Console.WriteLine("Отключено.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtIP.Text = GetLocalIpAddress() + @":6000";

        }
        public Dictionary<int, string> code = new Dictionary<int, string>()
        {
            [192] = "ю",
            [193] = "а",
            [194] = "б",
            [195] = "ц",
            [196] = "д",
            [197] = "е",
            [198] = "ф",
            [199] = "г",
            [200] = "х",
            [201] = "и",
            [202] = "й",
            [203] = "к",
            [204] = "л",
            [205] = "м",
            [206] = "н",
            [207] = "о",
            [208] = "п",
            [209] = "я",
            [210] = "р",
            [211] = "с",
            [212] = "т",
            [213] = "у",
            [214] = "ж",
            [215] = "в",
            [216] = "ь",
            [217] = "ы",
            [218] = "з",
            [219] = "ш",
            [220] = "э",
            [221] = "щ",
            [222] = "ч",
            [223] = "ъ",
            [224] = "Ю",
            [225] = "А",
            [226] = "Б",
            [227] = "Ц",
            [228] = "Д",
            [229] = "Е",
            [230] = "Ф",
            [231] = "Г",
            [232] = "Х",
            [233] = "И",
            [234] = "Й",
            [235] = "К",
            [236] = "Л",
            [237] = "М",
            [238] = "Н",
            [239] = "О",
            [240] = "П",
            [241] = "Я",
            [242] = "Р",
            [243] = "С",
            [244] = "Т",
            [245] = "У",
            [246] = "Ж",
            [247] = "В",
            [248] = "Ь",
            [249] = "Ы",
            [250] = "З",
            [251] = "Ш",
            [252] = "Э",
            [253] = "Щ",
            [254] = "Ч",
            [255] = "Ъ"
        };
        private byte[] dict;

        private string Decode(byte[] dict)
        {
            string output = "";

            try
            {
                foreach (byte s in dict)
                {
                    int key = s;
                    if (!code.ContainsKey(key))
                    {
                        throw new KeyNotFoundException("Неверный ключ для декодирования");
                    }

                    output += code[key];
                }
                return output;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка: введены некорректные символы");
                return output;
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Ошибка: обнаружен неверный ключ для декодирования");
                return output;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при декодировании");
                return output;
            }
        }

        private byte[] Code(string str)
        {
            string path = "D:\\1.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    byte[] output = new byte[str.Length];

                    for (int i = 0; i < str.Length; i++)
                    {
                        if (!code.ContainsValue(str[i].ToString()))
                        {
                            throw new ArgumentException("Недопустимый символ для кодирования");
                        }

                        int pr = code.FirstOrDefault(x => x.Value == str[i].ToString()).Key;
                        output[i] = Byte.Parse(pr.ToString());

                    }

                    sw.Write(output);
                    return output;
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Ошибка: обнаружен недопустимый символ");
                return null;
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка записи в файл");
                return null;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при кодировании");
                return null;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            textBox2.Text = Convert.ToString(byte.Parse(Decode(dict)));

            // textBox2.Text = Decode(textBox1.Text);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(byte.Parse(Decode(dict)));
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string str = "";
            byte[] bStr = Encoding.GetEncoding("koi8r").GetBytes(str);
            string str_new = Encoding.GetEncoding(1251).GetString(bStr);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }


}