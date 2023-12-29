import { username } from 'tmi.js/lib/utils';
import SaveDataHandler from './savedatahandler';
import tmi from 'tmi.js';

export default class ChatHandler {
    static client = null;

    static __settings = {};

    static onTalkList = Array();

    static __characters = Array();

    static onStatusList = Array();

    static __currentstatus = "";

    static {
        SaveDataHandler.onSettingsChanged((newsettings) => {
            this.__settings = newsettings;
            if (this.__settings.channel) {
                this.Init();
            }
        });

        SaveDataHandler.onCharacterChanged((slot, data) => {
            this.__characters[slot] = data;
        });

        window.testchat = this.Test;
    }


    static Init() {
        if (this.client) {
            this.client.disconnect();
        }

        this.client = new tmi.Client({
            options: { debug: true },
            channels: [this.__settings.channel]
        });
        this.client.connect().catch(this.triggerStatus);

        // Bind the message having been received
        this.client.on('message', this.receiveMessage);
        this.client.on('connecting', () => this.triggerStatus("Connecting!"));
        this.client.on('connected', () => this.triggerStatus("Connected"))
        this.client.on('disconnected', (reason) => this.triggerStatus("Disconnected: " + reason))
        this.client.on('reconnect', () => this.triggerStatus("Reconnecting.."));
        this.client.on('_promiseJoin', (message) => {
            if (message != null && message.startsWith("No response from Twitch")) { 
                this.triggerStatus("Connected, but got no response from Twitch. This usually means that the channel name is wrong");
            }
        });
    }

    static onTalk(func) {
        this.onTalkList.push(func);
    }

    static onStatus(func) {
        this.onStatusList.push(func);
        func(this.__currentstatus);
    }

    static triggerStatus(message) {
        console.log("Triggered status");
        this.__currentstatus = message;
        this.onStatusList.forEach((callback) => {
            callback(message);
        });
    }
    
    // This is the function that is run when a message is received
    /*eslint no-unused-vars: ["error", { "args": "none" }]*/
    static receiveMessage(channel, tags, message, self) {
        // In message, we get.. the message.
        // In tags, we get a whole lot of information about the user.
        console.log(message);
        console.log(tags);

        // I hate that this is the simplest version to split a string in two and keep the remainder.
        var index = message.indexOf(" ");
        var command = message === -1 ? message : message.substring(0, index);
        var restofmessage = index === -1 ? "" : message.substring(index + 1);

        var character_index = -1;
        var is_command = null;

        console.log(ChatHandler.__settings);

        // If the command starts with the name configured in the settings (configuration.js) as speak_prefix, use the username to look up who to speak as
        if (command == ChatHandler.__settings.speak_prefix) {
            for (const slot in ChatHandler.__characters) {
                var usernames = ChatHandler.__characters[slot].username.split('/');

                usernames.forEach((username) => {
                    if (tags.username == username.trim()) {
                        character_index = slot;
                        is_command = false;
                    }
                });
            }

            if (is_command !== false) {
                return;
            }


        } else if(command[0] == '!'){ // Else, if we think that it's a command like !bill
            for (const slot in ChatHandler.__characters) {
                if (command == ChatHandler.__characters[slot].username) {
                    character_index = slot;
                    is_command = true;
                }
            }

            if (is_command !== true) {
                return;
            }
        } else {
            return;
        }

        console.log(is_command, tags, restofmessage);
        ChatHandler.onTalkList.forEach((callback) => {
            callback(character_index, !is_command ? tags["display-name"] : null, restofmessage, tags);
        });

    }

    static Test() {
        // Below, we are making a function that spams messages. This is the list of stuff in there.
        var spamlist = [];
        spamlist.push({ username: "mara", message: ChatHandler.__settings.speak_prefix + " woop", "display-name": "Mara-kun" });
        spamlist.push({ username: "abe", message: ChatHandler.__settings.speak_prefix + " fart", "display-name": "Abe-desu" });
        spamlist.push({ username: "slightlytango", message: "!abc tentacleeeees. tentacles!", "display-name": "Hello" });
        spamlist.push({
            username: "slightlytango", message: `!abc According to all known laws of aviation, there is no way a bee should be able to fly.
Its wings are too small to get its fat little body off the ground.
The bee, of course, flies anyway because bees don't care what humans think is impossible.
Yellow, black. Yellow, black. Yellow, black. Yellow, black.
Ooh, black and yellow!
Let's shake it up a little.
Barry! Breakfast is ready!
Coming!
Hang on a second.
Hello?
Barry?
Adam?
Can you believe this is happening?
I can't.
I'll pick you up.
Looking sharp.`, "display-name": "Hello"
        });
        spamlist.forEach(value => ChatHandler.receiveMessage("", { username: value.username, "display-name": value["display-name"] }, value.message, null))
    }



}