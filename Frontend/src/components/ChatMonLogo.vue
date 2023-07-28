<template>
    <div class="logo">
        <h1>ChatMon</h1>
        <img src="assets/pokeball.svg" class="pokeball" />
    </div>

</template>

<script setup>

</script>

<style>
    .logo {
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .logo h1 {
        float: left;
        display: inline;
        margin: 0;
        vertical-align: middle;
        font-family: "Pokemon R/S", Arial, Helvetica, sans-serif;
        font-size: 7em;
        text-shadow: 0.00em 0.00em 0.10em #CCCCCCCC;
        z-index: 4;
    }

    @keyframes wiggleball {
        0% {
            transform: rotate(-25deg);
        }

        50% {
            transform: rotate(25deg);
        }

        100% {
            transform: rotate(-25deg);
        }
    }

    .pokeball {
        float: left;
        width: 20%;
        height: auto;
        max-height: 100%;
        display: inline-block;
        vertical-align: middle;
        margin-left: -4em;
        animation-name: wiggleball;
        animation-duration: 4s;
        animation-iteration-count: infinite;
    }
</style>