export default class InfiniteFusion {
    _personlist = null;
    _personlist_idmap = null;


    async personlist() {
        var pokedex_url = 'http:///downloads.example/infinitefusion/out.json';
        if (this._personlist !== null)
            return this._personlist;
        var response = await fetch(pokedex_url);
        var data = await response.json();

        this._personlist = [{ "value": "0", "label": "None" }];
        this._personlist_idmap = {};

        data.forEach((line, index) => {
            this._personlist_idmap[line[0]] = { id: line[0], tag: line[1], name: line[2] };
            this._personlist.push({ "value": line[0], "label": line[2] });
        }, this);

        return this._personlist;
    }

    async resolveImage(id) {
        if (this._personlist_idmap == null)
            await this.personlist();

        if (!id || id == "0")
            return "assets/pokeball.svg";

        const tag = this._personlist_idmap[id].tag;
        if (!tag.match(/B\d*H\d*/)) { // Normal, non fused image
            return await window.chrome.webview.hostObjects.infinitefusion.ResolveImage(id);
        } else {
            return await window.chrome.webview.hostObjects.infinitefusion.ResolveImage(this._personlist_idmap[id].tag);
        }
    }
}