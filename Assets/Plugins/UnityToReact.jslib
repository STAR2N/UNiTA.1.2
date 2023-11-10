
mergeInto(LibraryManager.library, {
    EnterSpace_React: function(spaceName) {
        window.dispatchReactUnityEvent(
            "EnterSpace",
            Pointer_stringify(spaceName)
        )
    },

    ExitSpace_React: function() {
        window.dispatchReactUnityEvent(
            "ExitSpace"
        )
    },

    JoinRoom_React: function(roomCode, roomName, id) {
        window.dispatchReactUnityEvent(
            "JoinRoom",
            Pointer_stringify(roomCode),
            Pointer_stringify(roomName),
            Pointer_stringify(id)
        )
    },

    ExitRoom_React: function(roomCode, roomName) {
        window.dispatchReactUnityEvent(
            "ExitRoom",
            Pointer_stringify(roomCode),
            Pointer_stringify(roomName)
        )
    },

    RequestPlayVideo_React: function (objectName) {
         window.dispatchReactUnityEvent(
            "RequestPlayVideo",
            Pointer_stringify(objectName)
        )
    },

    onToggleVideo_React: function (isOn) {
         window.dispatchReactUnityEvent(
            "onToggleVideo",
            isOn
        )
    },

    onToggleMic_React: function (isOn) {
         window.dispatchReactUnityEvent(
            "onToggleMic",
            isOn
        )
    },

    onToggleAudio_React: function (isOn) {
         window.dispatchReactUnityEvent(
            "onToggleAudio",
            isOn
        )
    },

    checkEnterByLink_React: function () {
         window.dispatchReactUnityEvent(
            "checkEnterByLink"
        )
    },

    RequestCopyLink_React: function(code) {
         window.dispatchReactUnityEvent(
            "RequestCopyLink",
            Pointer_stringify(code)
        )
    },
    RequestShowExitReview_React: function() {
        window.dispatchReactUnityEvent(
            "RequestShowExitReview"
        )
    },
    RequestShowIngameReview_React: function() {
        window.dispatchReactUnityEvent(
            "RequestShowIngameReview"
        )
    },
    RequestAddLetter_React: function(email, content) {
        window.dispatchReactUnityEvent(
            "RequestAddLetter",
            Pointer_stringify(email),
            Pointer_stringify(content)
        )
    },
    RequestEmail_React: function() {
        window.dispatchReactUnityEvent(
            "RequestEmail"
        )
    },
    OpenStoreLink_React: function(url) {
        window.dispatchReactUnityEvent(
            "OpenStoreLink",
            Pointer_stringify(url)
        )
    },
    RequestNetworkServerIP_React: function() {
        window.dispatchReactUnityEvent(
            "RequestNetworkServerIP"
        )
    }
});