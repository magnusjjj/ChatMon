import { reactive } from 'vue';
import SaveDataHandler from './savedatahandler.js';

export default class TTSHandler {
    static __naiveVoiceList = reactive([]);
    static __settings = {};
    static __voices = {};

    static {
        console.log("Loading ttshandler");
        var synth = window.speechSynthesis;
        var voices = synth.getVoices();
        if (voices.length) this.populatevoices();
        synth.addEventListener("voiceschanged", () => { this.populatevoices() });

        SaveDataHandler.onSettingsChanged((newsettings) => {
            console.log("new settings in tts", newsettings);
            this.__settings = newsettings;
        });
    }

    static populatevoices() {
        this.__naiveVoiceList.length = 0;

        for (const prop of Object.getOwnPropertyNames(this.__voices)) {
            delete this.__voices[prop];
        }

        var synth = window.speechSynthesis;
        var voices = synth.getVoices();
        for (let i = 0; i < voices.length; i++) {
            var langtext = voices[i].default ? "default" : `${voices[i].name} (${voices[i].lang})`;
            this.__naiveVoiceList.push({ "value": langtext, "label": langtext });
            this.__voices[langtext] = voices[i];
        }
    }

    static getNaiveVoiceList() {
        return this.__naiveVoiceList;
    }


    static speak(tospeak, voice)
    {
        var synth = window.speechSynthesis;
        var utterThis = new SpeechSynthesisUtterance(tospeak);
        utterThis.voice = this.getVoice(voice);
        console.log("In speak", this.__settings);

        utterThis.volume = this.__settings.voice_level / 100;
        synth.speak(utterThis);
    }

    static getVoice(voice) {
//        console.log("tried to get voice ", voice, this.__naiveVoiceList);
        return this.__voices[voice];
    }

    static shutUp() {
        window.speechSynthesis.cancel();
    }
}