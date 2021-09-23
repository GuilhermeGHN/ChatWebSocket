(function () {
    var uri = "wss://" + window.location.host + "/ws";
    var userId = null;

    _connect = function() {
        socket = new WebSocket(uri);

        socket.onopen = function (event) {
            //_addMessageLayout('#templateConnected', null);
            console.log("opened connection to " + uri);

            console.log(event);
        };

        socket.onclose = function (event) {
            console.log("closed connection from " + uri);
        };

        socket.onmessage = function (event) {
            _receiveMessage(event.data);            
            console.log(event.data);
        };

        socket.onerror = function (event) {
            console.log("error: " + event.data);
        };
    }

    _sendMessage = function () {
        var typeMessage = $('#txtTypeMessage').val();

        if (!typeMessage)
            return;

        console.log(typeMessage);
        socket.send(typeMessage);

        //var user = $('#userName').val();
        //var meetingId = $('#meetingId').val();

        //connectionHub.invoke('SendMessage', meetingId, user, typeMessage)
        //    .then(function () {
        //        $('#txtTypeMessage').val('');
        //        scrollDivMessage();
        //    })
        //    .catch(function (err) {
        //        return console.error(err.toString());
        //    });
    };

    _receiveMessage = function (data) {
        let message = JSON.parse(data);

        switch (message.Type) {
            case 'UserConnect':
                _addMessageLayout('#templateUserConnected', null);
                userId = message.Value;
                break;

            case 'OtherUserConnect':
                _addMessageLayout('#templateOtherUserConnected', message.Value);
                break;

            default:
                _addMessageLayout(message.SenderId === userId
                    ? '#templateSender'
                    : '#templateReceiver',
                    message.Value);
                break;
		}
    };

    //_start = function () {
    //    $('#btnSend').prop('disabled', true);
    //    $('#btnSend').prop('disabled', false);

    //    _addMessageLayout('#templateConnected', null);

        
    //    _addMessageLayout('#templateSender', 'Sender!!!');
    //    _addMessageLayout('#templateReceiver', 'Receiver!!!');

    //    _addMessageLayout('#templateSender', 'Sender!!!');
    //    _addMessageLayout('#templateReceiver', 'Receiver!!!');

    //    _addMessageLayout('#templateSender', 'Sender!!!');
    //    _addMessageLayout('#templateReceiver', 'Receiver!!!');

    //    _addMessageLayout('#templateSender', 'Sender!!!');
    //    _addMessageLayout('#templateReceiver', 'Receiver!!!');
        
    //}

    _addMessageLayout = function (templateName, message) {
        const template = $(templateName).clone().prop('content');
        let templateContent = $(template).find('.direct-chat-msg');

        $(templateContent).find('.direct-chat-text').html(message);
        templateContent.appendTo('.direct-chat-messages');
    }

    _scrollDivMessage = function () {
        $('.direct-chat-messages').scrollTop($('.direct-chat-messages')[0].scrollHeight);
    }

    $('#btnSend').click(function (evt) {
        _sendMessage();
        evt.preventDefault();
    });

    $('#txtTypeMessage').keyup(function (e) {
        if (e.keyCode == 13)
            _sendMessage();
    });

    $(document).ready(_connect);
})();

