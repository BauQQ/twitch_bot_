using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects
{
    public class CommandList
    {
        public Dictionary<string, Command> table { get; set; } = new Dictionary<string, Command>();

        /*
         *  {

          { "!mycommands", new Command {
          enabled = true,
          name = "test",
          dcmd = "commands",
          cmdAliases = new List<string>()
          {
              "mycommands",
              "mycommand"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "",
          locked = new List<string>()
          {
              "luffaow"
          }
          }},

          { "!purpose", new Command {
          enabled = true,
          name = "purpose",
          dcmd = "purpose",
          cmdAliases = new List<string>()
          {
              "purpose"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "I have no purpose, yet...",
          locked = new List<string>()
          {
          }
          }},

          { "!rps", new Command {
          enabled = true,
          name = "Rock Paper Siccors",
          dcmd = "rps",
          cmdAliases = new List<string>()
          {
              "rps"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd} ${query} ${rounds} ${wager}",
          locked = new List<string>()
          {
          }
          }},

          { "!rpsaccept", new Command {
          enabled = true,
          name = "Rock Paper Siccors",
          dcmd = "rps",
          cmdAliases = new List<string>()
          {
              "rpsaccept"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!rpsdeny", new Command {
          enabled = true,
          name = "Rock Paper Siccors",
          dcmd = "rps",
          cmdAliases = new List<string>()
          {
              "rpsdeny"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!rpscancel", new Command {
          enabled = true,
          name = "Rock Paper Siccors",
          dcmd = "rps",
          cmdAliases = new List<string>()
          {
              "rpscancel"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!berganderf", new Command {
          enabled = true,
          name = "Berganderf",
          dcmd = "berganderf",
          cmdAliases = new List<string>()
          {
              "berganderf",
              "Berganderf"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "Hello Sir ${user}, welcome back to the channel, i hope you have a nice day.",
          locked = new List<string>()
          {
              "berganderf",
              "luffaow"
          }
          }},

          { "!demo", new Command {
          enabled = true,
          name = "demo",
          dcmd = "demo",
          cmdAliases = new List<string>()
          {
              "demo"
          },
          userLevel = 0,
          ucd = 20,
          gcd = 5,
          cost = 0,
          respons = "${user} this is a demo command!",
          locked = new List<string>()
          {
          }
          }},

          { "!math", new Command {
          enabled = true,
          name = "math test",
          dcmd = "math",
          cmdAliases = new List<string>()
          {
              "math"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd} ${number} ${number2}",
          locked = new List<string>()
          {
          }
          }},

          { "!RRAccept", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
                "rraccept",
                "Rraccept",
                "rRaccept"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!RRdeny", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
                "rrdeny",
                "Rrdeny",
                "rRdeny"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!RRshoot", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
                "Rrshoot",
                "rRshoot",
                "rrshoot"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!RRspin", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
                "Rrspin",
                "rRspin",
                "rrspin"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!RRcancel", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
            "Rrcancel",
            "rRcancel",
            "rrcancel"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
          }
          }},

          { "!rradmcancel", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
            "Rradmcancel",
            "rRadmcancel",
            "rradmcancel"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd}",
          locked = new List<string>()
          {
              "luffaow"
          }
          }},

          { "!RRoullete", new Command {
          enabled = true,
          name = "Russian Roullete",
          dcmd = "rroullete",
          cmdAliases = new List<string>()
          {
            "RRoullete",
            "Rroullete",
            "rRoullete",
            "rroullete"
          },
          userLevel = 0,
          ucd = 0,
          gcd = 0,
          cost = 0,
          respons = "${dcmd} ${target} ${gunsize} ${bullets} ${wager}",
          locked = new List<string>()
          {
          }
          }}

        }
         */
    }
}
