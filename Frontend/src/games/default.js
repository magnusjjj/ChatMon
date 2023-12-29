export default class DefaultGame{
    async personlist() {
        var pokedex_url = 'http:///downloads.example/game/pokemon/pokemon.json-master/pokedex.json';
        var response = await fetch(pokedex_url);
        var data = await response.json();
        var returner = data.map(line => {
            return ({ "value": line.id, "label": line.name.english });
        });
        returner.unshift({ "value": "0", "label": "None" });
        return returner;
    }
    async resolveImage(id) {

        return id && id != 0 ? "http:///downloads.example/game/pokemon/pokemon.json-master/thumbnails/" + id.toString().padStart(3, '0') + ".png" : "/assets/pokeball.svg";
    }
}