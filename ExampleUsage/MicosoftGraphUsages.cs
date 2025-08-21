using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MIcrosoftGraphService.ExampleUsage
{
    internal class MicosoftGraphUsages
    {
        /*
         Vedno prisotni parametri:
         ClientId,
                    SecretValue,
                    TenantId,
                    DefaultScope 

          Rabmo funkcionalnost: 
            - get mails from mailbox (parametri za select/filter messages) - vrača messageid, subject, date, from ,to, cc, bcc,...

                messages = graphServiceClient
                    .Users[i4.Utils.SiGot.Nkbm.Parameters.ATMFault.Get.MailboxId]
                    .Messages
                    .GetAsync(x => {
                        x.QueryParameters.Top = 1000;
                    }).Result.Value;


            - get mail details (parameter: messageIds) - vrača celoten message (tudi body, attachments,...)

            - delete mails (parameter: messageIds) - briše mail iz nabiralnika

         graphServiceClient
                        .Users[i4.Utils.SiGot.Nkbm.Parameters.ATMFault.Get.MailboxId]
                        .Messages[message.Id]
                        .DeleteAsync();

        */

        /*
         Potem pa še - General sending mail 
        
        private void SendMessage(MailSettings settings, IEnumerable<RecipientMailAddress> mailAddresses)
        {

            var message = new Microsoft.Graph.Models.Message()
            {
                Body = new ItemBody()
            };
            // Set and init message props
            message.Subject = settings.Subject;
            message.Body.Content = settings.Html;
            message.Body.ContentType = BodyType.Html;
            message.ToRecipients = new List<Recipient>();
            message.CcRecipients = new List<Recipient>();
            message.BccRecipients = new List<Recipient>();
            message.ReplyTo = new List<Recipient>();
            message.Attachments = new List<Attachment>();

            // Loop hrough addresses(To, Cc, Bcc)
            var sendIds = "";

            foreach (var addr in mailAddresses)
            {
                if (addr.SendOption == SendOptionTypes.To || addr.SendOption == SendOptionTypes.ToBatch)
                {
                    var rec = new Recipient();
                    rec.EmailAddress = new EmailAddress();
                    rec.EmailAddress.Address = addr.GetAddress();

                    message.ToRecipients = message.ToRecipients.Append(rec).ToList();
                }
                else if (addr.SendOption == SendOptionTypes.Cc)
                {
                    var rec = new Recipient();
                    rec.EmailAddress = new EmailAddress();
                    rec.EmailAddress.Address = addr.GetAddress();

                    message.CcRecipients = message.CcRecipients.Append(rec).ToList();
                }
                else if (addr.SendOption == SendOptionTypes.Bcc)
                {
                    var rec = new Recipient();
                    rec.EmailAddress = new EmailAddress();
                    rec.EmailAddress.Address = addr.GetAddress();

                    message.BccRecipients = message.BccRecipients.Append(rec).ToList();
                }
            }

            if (!String.IsNullOrEmpty(settings.ReplyToMail))
            {
                var rt = new Recipient();
                rt.EmailAddress = new EmailAddress();
                rt.EmailAddress.Address = settings.ReplyToMail;

                message.ReplyTo = message.ReplyTo.Append(rt).ToList();
            }

            if (settings.RequestReadConformation)
            {
                message.IsReadReceiptRequested = true;
            }

            if (settings.RequestDeliveryConformation)
            {
                message.IsDeliveryReceiptRequested = true;
            }

            // Add attachments
            foreach (MailAttachment att in settings.Attachments)
            {
                if (!String.IsNullOrEmpty(att.FullFilename))
                {

                    message.Attachments.Add(new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        ContentBytes = System.IO.File.ReadAllBytes(att.FullFilename),
                        Name = att.Name,
                        ContentType = MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(att.Name.ToLower()))
                    });
                }
                else if (att.Buffer != null)
                {
                    message.Attachments.Add(new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        ContentBytes = att.Buffer,
                        Name = att.Name,
                        ContentType = MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(att.Name.ToLower()))
                    });
                }
            }

            var authenticationProvider = new Microsoft.Kiota.Abstractions.Authentication.BaseBearerTokenAuthenticationProvider(
                new i4.Utils.Notification.Mail.MicrosoftGraph.TokenProvider(
                    ClientId,
                    SecretValue,
                    TenantId,
                    new[] { i4.Utils.Notification.Parameters.MicrosoftGraph.Get.DefaultScope }));
            var graphServiceClient = new GraphServiceClient(authenticationProvider);

            var request = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody();
            request.Message = message;
            request.SaveToSentItems = true;

            graphServiceClient.Users[UserId].SendMail.PostAsync(request).RunSync();

        }*/



    }
}
