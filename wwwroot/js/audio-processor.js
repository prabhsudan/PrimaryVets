class AudioStreamProcessor extends AudioWorkletProcessor {
    constructor() {
        super();
    }

    process(inputs, outputs, parameters) {
        const input = inputs[0];
        if (!input || !input[0]) return true;

        const channelData = input[0];
        const buffer = new Int16Array(channelData.length);

        for (let i = 0; i < channelData.length; i++) {
            const s = Math.max(-1, Math.min(1, channelData[i]));
            buffer[i] = s < 0 ? s * 0x8000 : s * 0x7fff;
        }

        // Transfer the ArrayBuffer to avoid copying
        this.port.postMessage(buffer.buffer, [buffer.buffer]);

        return true;
    }
}

registerProcessor('audio-stream-processor', AudioStreamProcessor);