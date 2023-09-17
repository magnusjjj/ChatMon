<template>
    <!-- The preloader. Nice little download bars and wiggling pokeball -->
    <ChatMonPreLoader v-if="ShouldShowPreloader"/>
    <div v-else class="mainwindow">
        <div style="height: 4em;">
            <n-button class="dragmeh" size="large" type="primary" v-if="ShouldShowUI">
                Drag the window here
                <template #icon>
                    <n-icon><drag-outlined /></n-icon>
                </template>
            </n-button>
            <n-button class="close" size="large" type="primary" v-if="ShouldShowUI" @click="window.close">
                <template #icon>
                    <n-icon><close-outlined /></n-icon>
                </template>
            </n-button>
        </div>
        <div class="content">
            <PersonSlot v-for="(person, index) in PersonList" :key="index" :person="person" :slotnumber="index" @CharacterPictureClicked="OnCharacterClicked" :message="MessageList[index]" />
            <PersonConfigurator :show="ShouldShowConfigurator" :personslot="ConfiguratorSlot" :person="ConfiguratorPerson" @SaveCompleted="ShouldShowConfigurator = false" @Cancelled="ShouldShowConfigurator = false" />
            <ChatMonSettings :show="ShouldShowSettings" @SaveCompleted="ShouldShowSettings = false" @Cancelled="ShouldShowSettings = false" />

        </div>
        <div class="ShowSettingsButton" v-if="ShouldShowUI">
            <n-button @click="ShouldShowSettings = true" size="large" type="primary">
                Settings
                <template #icon>
                    <n-icon><setting-outlined /></n-icon>
                </template>
            </n-button>
            <n-button size="large" @click="ShouldShowAbout = true" type="primary">
                About
                <template #icon>
                    <n-icon><question-outlined /></n-icon>
                </template>
            </n-button>

            <TwitchStatus />
        </div>
        <ChatMonAbout :show="ShouldShowAbout" @Close="ShouldShowAbout = false" />
    </div>
</template>

<script setup>
    // Alright. We need to grab a bunch of code from different javascript modules..
    import { ref, reactive } from 'vue'; // Like how to define variables for the template above

    import { SettingOutlined, DragOutlined, CloseOutlined, QuestionOutlined } from '@vicons/antd'; // Icons, for different buttons and the like...

    // App.vue handles all sorts of things. We need to be able to save data, read from chat, speak out loud and mess with the chatmon window.
    import SaveDataHandler from './handlers/savedatahandler';
    import ChatHandler from './handlers/chathandler';
    import TTSHandler from './handlers/ttshandler';
    import WebView2IntegrationHandler from './handlers/webview2integrationhandler'; // This lets us talk to the backend, the C# side of things (the 'ChatMon' projekt)

    // Next up, we define a whole slew of variables for the template above. These deal with whether or not certain windows and things should be visible.
    const ShouldShowPreloader = ref(true);
    const ShouldShowUI = ref(true); // This one is for hiding or showing parts of the UI when the window is not focused
    const ShouldShowConfigurator = ref(false); // The window when someone clicks a chatmon slot
    const ShouldShowSettings = ref(false);
    const ShouldShowAbout = ref(false);

    var PersonList = reactive([{}, {}, {}, {}, {}, {}]); // This is the list of chatmon
    var MessageList = reactive(["", "", "", "", "", ""]); // A list of the messageboxes below the chatmon
    var ConfiguratorPerson = ref({}); // This will be filled with the data of a chatmon clicked
    var ConfiguratorSlot = ref(0); // And this with the slot number clicked.

    // We have a button-like thing in the program that we want to use to be able to drag the window. This breaks some things.
    // The following two events fixes it up.
    // We can't use the :focus css, because then we can't use the --app-region drag, due to OS-level issues. Unfixable.
    // We want to be able to use --app-region drag, so that we can drag around the window with the dragme box.
    // This means that we need to hide and show the UI via this hack instead.
    addEventListener("blur", (event) => {
        ShouldShowUI.value = false;
    });
    addEventListener("focus", (event) => {
        ShouldShowUI.value = true;
    });


    // When the user saves settings, we need to tell the C# backend about it, mostly for the keybind to shut up a pokemon
    // This needs to be this far up, the preloader needs to know what to do.
    SaveDataHandler.onSettingsChanged(function (settings) {
        WebView2IntegrationHandler.SendMessage({ "type": "ChatMonSettings", "settings": settings });
    });

    // When we are done with setting everything else up, it's time to show our nifty preloader.
    // We send a message to the C# backend to start downloading the pictures for all the pokemon, all that jazz.
    WebView2IntegrationHandler.SendMessage({"type": "StartPreloader"});

    // We have a couple of functions here for actions in the program. This is, like the name suggests, the function for what happens if you click a pokemon
    function OnCharacterClicked(slotnumber) {
        ConfiguratorSlot.value = slotnumber;
        ConfiguratorPerson.value = PersonList[slotnumber];
        ShouldShowConfigurator.value = true;
    }

    // The handler for what should be done if a character cahnges..
    SaveDataHandler.onCharacterChanged((slot, data) => {
        PersonList.splice(slot, 1, data);
    });


    // If there is an error in the stored data for settings, show the settings popup
    SaveDataHandler.onValidationError(() => {
        ShouldShowSettings.value = true;
    });

    // Time for the main course, what we do if someone sends a chatmessage
    ChatHandler.onTalk((slot, displayname, message) => {
        if (displayname != null && displayname != PersonList[slot].displayname) {
            // Is it something like !bob, or a !poke? If the latter, change the visible username to how the username looks in chat. For example, SlightlyTango instead of slightlytango
            PersonList[slot].displayname = displayname;
            SaveDataHandler.SaveCharacter(slot, PersonList[slot]); // Save the change for if we want to restart chatmon
        }

        MessageList[slot] = message; // Display the message

        TTSHandler.speak(message, PersonList[slot].voice); // And talk!
    });

    // Finally, the keybind to shut up any TTS in progress needs to run.
    WebView2IntegrationHandler.onMessage('ShutUp', () => {
        TTSHandler.shutUp(); // Shut up the TTS

        for (let i = 0; i < MessageList.length; i++) { // And remove all the visible messages
            MessageList[i] = "";
        }
    });

    // Preloader is done!
    WebView2IntegrationHandler.onMessage('SetupDone', () => {
        ShouldShowPreloader.value = false;
    });
</script>

<style>
    @font-face {
        font-family: "Pokemon R/S";
        src: url("/public/assets/pokemon_fire_red.woff") format('woff');
    }

    .dragmeh {
        --app-region: drag;
        -webkit-app-region: drag;
        margin-left: 1em;
        float: left;
    }

    html, body, #app {
        position: relative;
        width: 100%;
        height: 100%;
        padding: 0;
        margin: 0;
    }
</style>

<style scoped>
    .mainwindow {
       position: relative;
       height: 100%;
       width: 100%;
    }

    .ShowSettingsButton {
        clear: both;
        position: relative;
        top: 1em;
    }

    .ShowSettingsButton button {
        margin-left: 1em;
        margin-bottom: 0.5em;
    }

    .close {
        float: right;
    }
</style>