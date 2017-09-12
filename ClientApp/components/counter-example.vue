<template>
    <div>
        <h1>Counter</h1>

        <p>This is a simple example of a Vue.js component & SignalR. To see how this data is coming from the server, open this page in more than one browser tab.  You will notice how the count is synchronized between the two, because the data is being pushed to each client from the server.</p>

        <p>
            Auto count: <strong>{{ count }}</strong>
        </p>

    </div>
</template>

<script>
    var signalR = require('../signalr-client.min.js');

    let http = new signalR.HttpConnection('http://' + document.location.host + '/counter');
    let connection = new signalR.HubConnection(http);

    export default {
        data() {
            return {
                count: 0,
            }
        },
        mounted: function () {
            connection.start();
            
            connection.on('increment', data => {
                this.count = data;
            });
        }
    }
</script>

<style>
</style>
