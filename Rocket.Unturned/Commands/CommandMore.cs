﻿using System;
using Rocket.API;
using Rocket.API.Chat;
using Rocket.API.Commands;
using Rocket.API.I18N;
using Rocket.Core.Commands;
using Rocket.Core.I18N;
using Rocket.Unturned.Player;

namespace Rocket.Unturned.Commands
{
    public class CommandMore : ICommand
    {
        public bool SupportsCaller(Type commandCaller)
        {
            return typeof(UnturnedPlayer).IsAssignableFrom(commandCaller);
        }

        public void Execute(ICommandContext context)
        {
            ITranslationLocator translations = ((UnturnedImplementation)context.Container.Resolve<IImplementation>()).ModuleTranslations;
            IChatManager chatManager = context.Container.Resolve<IChatManager>();

            if(context.Parameters.Length != 1)
                throw new CommandWrongUsageException();

            byte amount = context.Parameters.Get<byte>(0);

            UnturnedPlayer player = (UnturnedPlayer)context.Caller;
            ushort itemId = player.Player.equipment.itemID;

            if (itemId == 0)
            {
                chatManager.SendLocalizedMessage(translations, player, "command_more_dequipped");
                return;
            }

            chatManager.SendLocalizedMessage(translations, player, "command_more_give", amount, itemId);
            player.GiveItem(itemId, amount);
        }

        public string Name => "More";
        public string Summary => "Gives more of an item that you have in your hands.";
        public string Description => null;
        public string Permission => "Rocket.Unturned.More";
        public string Syntax => "<amount>";
        public ISubCommand[] ChildCommands => null;
        public string[] Aliases => null;
    }
}