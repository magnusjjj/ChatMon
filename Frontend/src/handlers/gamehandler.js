import SaveDataHandler from "./savedatahandler";
import DefaultGame from "../games/default";
import InfiniteFusion from "../games/infinitefusion";

export default class GameHandler {
    static _settings = null;
    static _currentgame = null;

    static {
        SaveDataHandler.onSettingsChanged((newsettings, oldsettings) => {
            this._settings = newsettings;
            if (this._settings["gametype"] == 'default') {
                this._currentgame = new DefaultGame();
            }
            if (this._settings["gametype"] == 'infinitefusion') {
                this._currentgame = new InfiniteFusion();
            }
        });        
    }

    static currentGame() {
        return this._currentgame;
    }
}