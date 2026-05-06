namespace LeveGestao.Application.Interfaces;

public interface IEmailService
{
    Task EnviarAsync(string destinatario, string nomeDestinatario, string assunto, string corpo);
}
