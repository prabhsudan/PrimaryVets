window.mediaRecorder = null;
window.audioChunks = [];
window.mediaStream = null;
window.currentFieldId = null;

window.notesRecorder = null;
window.notesStream = null;
window.notesChunks = [];
window.isNotesRecording = false;

// GLOBAL SAFETY CHECK
window.isAnyRecording = function () {
    return (
        window.mediaRecorder?.state === "recording" ||
        window.notesRecorder?.state === "recording"
    );
};

// STREAM CLEANUP HELPER
function stopStream(stream) {
    if (stream) {
        stream.getTracks().forEach(t => t.stop());
    }
}

// START MAIN VOICE RECORDING
window.startVoice = async function (inputId, btn) {
    try {
        if (window.isAnyRecording()) {
            console.warn("Another recording is already active.");
            return;
        }

        window.currentFieldId = inputId;
        window.audioChunks = [];

        window.mediaStream = await navigator.mediaDevices.getUserMedia({
            audio: true
        });

        window.mediaRecorder = new MediaRecorder(window.mediaStream);

        window.mediaRecorder.ondataavailable = function (event) {
            if (event.data && event.data.size > 0) {
                window.audioChunks.push(event.data);
            }
        };

        window.mediaRecorder.onstop = async function () {

            stopStream(window.mediaStream);
            window.mediaStream = null;

            const audioBlob = new Blob(window.audioChunks, {
                type: "audio/webm"
            });

            await window.uploadAudioFile(audioBlob);
        };

        window.mediaRecorder.start(250);

        btn?.classList.add("mic-active");
        const textSpan = document.getElementById("micBtnText");
        if (textSpan) textSpan.innerHTML = "Listening...";

        document.getElementById("stopBtn")?.classList.remove("d-none");

    } catch (err) {
        console.error("Microphone initialization failed:", err);
    }
};

// STOP MAIN VOICE RECORDING
window.stopVoice = function () {
    try {
        if (window.mediaRecorder?.state === "recording") {
            window.mediaRecorder.stop();
        }

        const btn = document.getElementById("micBtn");
        if (btn) {
            btn.classList.remove("mic-active");
            const textSpan = document.getElementById("micBtnText");
            if (textSpan) textSpan.innerHTML = "Processing File...";
        }

        document.getElementById("stopBtn")?.classList.add("d-none");

    } catch (err) {
        console.error("Stop voice error:", err);
    }
};
// ===============================
// UPLOAD MAIN AUDIO
window.uploadAudioFile = async function (audioBlob) {
    try {
        const formData = new FormData();
        formData.append("audioFile", audioBlob, "pet_registration_speech.webm");

        const response = await fetch('/PetRegistration/ProcessAudioFile', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error(`HTTP error: ${response.status}`);
        }

        const data = await response.json();

        window.autofillFormFields(data);

    } catch (err) {
        console.error("Upload failed:", err);
    } finally {
        const btn = document.getElementById("micBtn");
        if (btn) {
            const textSpan = document.getElementById("micBtnText");
            if (textSpan) textSpan.innerHTML = "Speak";
        }
        window.currentFieldId = null;
    }
};

// AUTOFILL LOGIC
window.autofillFormFields = function (data) {
    if (!data) return;

    function getValue(obj, key) {
        const target = key.toLowerCase().replace(/[^a-z0-9]/g, "");
        for (let k in obj) {
            if (k.toLowerCase().replace(/[^a-z0-9]/g, "") === target) {
                return obj[k];
            }
        }
        return null;
    }

    function setInput(id, key) {
        const el = document.getElementById(id);
        const value = getValue(data, key);
        if (!el || value == null) return;

        el.value = value;
        el.dispatchEvent(new Event("input", { bubbles: true }));
        el.dispatchEvent(new Event("change", { bubbles: true }));
    }

    function setDropdown(id, key) {
        const el = document.getElementById(id);
        const value = getValue(data, key);
        if (!el || !value) return;

        const norm = value.toString().toLowerCase().trim();

        const option = Array.from(el.options).find(o =>
            o.value.toLowerCase().trim() === norm ||
            o.text.toLowerCase().trim() === norm
        );

        el.value = option ? option.value : "";
        el.dispatchEvent(new Event("change", { bubbles: true }));
    }

    setInput("Name", "name");
    setInput("PetName", "petName");
    setDropdown("Species", "species");
    setDropdown("Gender", "gender");
    setDropdown("Treatment", "treatment");
    setInput("Age", "age");
    setInput("BodyTemperature", "bodyTemperature");
    setInput("Weight", "weight");
    setInput("Breed", "breed");
    setInput("Mobile", "mobile");
    setDropdown("PetBehaviour", "petBehaviour");
};

// NOTES RECORDING
window.startNotesVoice = async function (btn) {
    try {
        if (window.notesRecorder && window.notesRecorder.state === "recording") {
            console.warn("Notes recording already running.");
            return;
        }

        window.notesChunks = [];

        window.isNotesRecording = true;

        window.notesStream = await navigator.mediaDevices.getUserMedia({
            audio: true
        });

        window.notesRecorder = new MediaRecorder(window.notesStream);

        window.notesRecorder.ondataavailable = function (e) {
            if (e.data && e.data.size > 0) {
                window.notesChunks.push(e.data);
            }
        };

        window.notesRecorder.onstop = async function () {

            if (window.notesStream) {
                window.notesStream.getTracks().forEach(t => t.stop());
                window.notesStream = null;
            }

            const blob = new Blob(window.notesChunks, {
                type: "audio/webm"
            });

            try {
                const text = await uploadNotesOnly(blob);

                appendToNotes(text);
                document.getElementById("micNotesText").innerHTML = "Speak Notes";

            } catch (err) {
                console.error("Notes transcription failed:", err);
            }

            window.isNotesRecording = false;
        };

        window.notesRecorder.start(250);

        btn?.classList.add("mic-active");

        document.getElementById("micNotesText").innerText = "Listening...";

        document.getElementById("stopBtnNotes")?.classList.remove("d-none");

    } catch (err) {
        window.isNotesRecording = false;
        console.error("Notes mic error:", err);
    }
};

// STOP NOTES RECORDING
window.stopVoiceNotes = function () {
    try {

        if (window.notesRecorder?.state === "recording") {
            window.notesRecorder.stop();
        }

        const btn = document.getElementById("micNotesBtn");

        if (btn) {
            btn.classList.remove("mic-active");

            const textSpan = document.getElementById("micNotesText");

            if (textSpan) {
                textSpan.innerHTML = "Processing File...";
            }
        }

        document.getElementById("stopBtnNotes")
            ?.classList.add("d-none");

    } catch (err) {
        console.error("Stop notes voice error:", err);
    }
};
// NOTES UPLOAD

async function uploadNotesOnly(blob) {

    const formData = new FormData();

    formData.append("audioFile", blob, "notes.webm");

    formData.append("isNotesOnly", "true");

    const res = await fetch('/PetRegistration/ProcessAudioFile', {
        method: 'POST',
        body: formData
    });

    if (!res.ok) {
        throw new Error("Upload failed");
    }

    const data = await res.json();

    return (
        data.text ||
        data.transcript ||
        data.result ||
        data.Text ||
        ""
    );
}

// APPEND NOTES
function appendToNotes(text) {
    const el = document.getElementById("Notes");
    if (!el || !text) return;

    el.value += (el.value ? ". " : "") + text;
}