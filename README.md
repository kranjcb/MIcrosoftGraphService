# Microsoft Graph Service

Iz našega serverja bi spravil en del funkcionalnosti v en ločen proces (ala microservice). 
Funkcionalnost je uporaba Microsoft.Graph za različne operacije. 
Trenutno so to samo pošiljanje mailov (v splošnem) in čekiranje/management mailov enega poštnega predala. 
Kasneje bo še uporaba SharedPoint, ....
Za Microsoft.Graph uporabljamo tole: https://www.nuget.org/packages/Microsoft.Graph

Se pravi je treba narediti .Net 9 exe program MicrosoftGraphService. 

	- Komunikacija med serverjem in MicrosoftGraphService bo preko named pipes (že imamo sample kode). 
	- Komunikacija: Server bo po potrebi štartal MicrosoftGraphService, ta bo odprl NamedPipeServerStream, server se bo povezal na njega preko NamedPipeClientStream.
	- Komunikacija: Server pošlje vsebino na MicrosoftGraphService, ta jo sprocesira in pošlje nazaj odgovor. Ala tipični server. Kasneje bomo to predelali v REST API.
	- MicrosoftGraphService 2 možna args. Prvi je ime named pipes (default je kar MicrosoftGraphService), drugi je idle time (default 5 min). 
	- Če v idle time ne pride noben request po pipcah do MicrosoftGraphService se ta zapre.
	- Prenos med NamedPipeServerStream in NamedPipeClientStream je definiran. Prenaša se v byte-ih, ki se konvertirajo v string.
	- String se pa serializira/deserializira v .net objekte (by System.Text.Json)
	- Logiranje mora biti ustrezno rešeno. Lahko je to NLog, ali kaj druzga. 
	- Če ob startupu MicrosoftGraphService pride do napake, se ta zapiše in zapre proces z ExitCode = 1 (tako da bo server vedel)
	- Če ob procesiranju requesta pride do napake, to javi nazaj serverju v standardni obliki (imamo objekte za to... se pravi ta objekt deserializiran)
	 
Tu je primer oz. izsek iz enega obstoječega primera. Lahko se uporabi kot muštr, ni pa nujno, so tudi primeri kode.
Treba je narediti še unit teste & sonarqube zadeve... in klienta (testni C# projekt) da se bo povezal na to (tudi če bo kaj sprememb pri samih pipcah).

Za ClientId, Tennant, .... se lahko uporabiš kakšen testni account na Microsft Entra ID.
Tu ne vem točno, če ne bo šlo ti lahko jaz probam testne naredit.

V tem projektu se še uporablja Newtonsoft.Json, NLog za logiranje, .... 
Koda je kot je, verjetno ni optimalna, je bila delana za .net 3.5, potem dopolnjena za 4.5 in 4.8.. nekaj bi bilo verjetno še za predelat....
V MicosoftGraphUsages.cs so primeri uporabe, oz. navodila kaj mora bit podprto....
Kar je v Model bi bilo share-ano še s serverjem, da bodo isti objekti uporabljeni na obeh straneh.

## Project Structure

```
├── Program.cs              # Main entry point
├── Server.cs               # Main server implementation
├── ServerFunctions.cs      # Server function implementations
├── Model/
│   ├── RequestMessage.cs   # Request message model
│   └── ResponseMessage.cs  # Response message model
├── Shared/
│   ├── PipeClientAsync.cs  # Async pipe client
│   └── PipeServerAsync.cs  # Async pipe server
└── ExampleUsage/
    └── MicosoftGraphUsages.cs # Usage examples
```

