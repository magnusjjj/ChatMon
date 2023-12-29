export default class SaveDataHandler {
    static settingsChangedList = Array();
    static __settings = null;
    static characterChangedList = Array();
    static __characters = Array();
    static settingsValidationErrorList = Array();


    static settingValidators = {
        channel: (value) => { return value == null || value.length == 0 || value.startsWith("http://") || value.startsWith("https://") ? "Paste in your channel url" : null}, // Channel name, without url
        voice_level: (value) => { return value < 0 || value > 100 ? "The voice level can't be below 0 or above 100" : null }, // Voice level, 0-100
        speak_prefix: (value) => { return value == null || value.length == 0 ? "The prefix can't be empty. It needs to be something like !poke" : null }, // What a twitch chat message needs to start with for the chatbot to listen. Like !poke
        key_ctrl: (value) => { return null }, // The following three settings are for what key combination to hit to silence TTS and remove all messages.
        key_alt: (value) => { return null },
        key: (value) => { return null },
    };

    static defaultSettings = {
        voice_level: 50,
        key: 0,
        key_alt: false,
        key_ctrl: false,
        channel: '',
        gametype: 'default'
    };

    static {
        console.log("First loading settings");
        this.settingsChangedList = Array();
        this.__settings = JSON.parse(window.localStorage.getItem("settings")) ?? {};
        for (const key in this.defaultSettings) { 
            if (!(key in this.__settings)) {
                this.__settings[key] = this.defaultSettings[key];
            }
        }

        this.settingsChangedList.forEach((eventhandler) => eventhandler(this.__settings));
        this.validateSettings();
        
        for (const [key, value] of Object.entries(window.localStorage)) {
            if (key.startsWith("character")) {
                const theobject = JSON.parse(value);
                const slotnumber = parseInt(key.substring("character".length));
                this.characterChangedList.forEach((eventhandler) => eventhandler(slotnumber, theobject));
                this.__characters[slotnumber] = theobject;
            }
        }
    }

    static SaveCharacter(SlotNumber, value) {
        window.localStorage.setItem("character" + SlotNumber, JSON.stringify(value));
        const horriblehack = JSON.parse(JSON.stringify(value)); // Hack to remove horrible no good proxy objects :(
        this.characterChangedList.forEach((eventhandler) => eventhandler(SlotNumber, horriblehack));  
    }

    static onCharacterChanged(func) {
        this.characterChangedList.push(func);
        this.__characters.forEach((value, index) => {
            func(index, value);
        });
    }

    static SaveSettings(settings) {
        const oldsettings = this.__settings;
        window.localStorage.setItem("settings", JSON.stringify(settings));
        this.__settings = JSON.parse(JSON.stringify(settings)); // Make sure to strip any vue proxy nonsense
        this.settingsChangedList.forEach((eventhandler) => eventhandler(settings, oldsettings));
        this.validateSettings();
    }

    static onSettingsChanged(func) {
        this.settingsChangedList.push(func);
        func(this.__settings, this.__settings);
    }

    static validateSettings() {
        var validationerrors = {};
        console.log(this.__settings);
        for (const key in this.settingValidators) {
            const validationresult = this.settingValidators[key](key in this.__settings ? this.__settings[key] : null);
            if (validationresult != null) {
                validationerrors[key] = validationresult;
            }
        }
        if (Object.keys(validationerrors).length) {
            this.settingsValidationErrorList.forEach((callback) => callback(validationerrors));
            console.log("Found validation errors");
        }
        console.log("validating", validationerrors);
    }

    static onValidationError(func) {
        this.settingsValidationErrorList.push(func);
        this.validateSettings();
    }
}