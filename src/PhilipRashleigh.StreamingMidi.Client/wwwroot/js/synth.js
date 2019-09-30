function Synth () {
    let context = new AudioContext(),
        oscillators = {};
    
    let playNote = frequency => {
        oscillators[frequency] = context.createOscillator();
        oscillators[frequency].frequency.value = frequency;
        oscillators[frequency].connect(context.destination);
        oscillators[frequency].start(context.currentTime);
    };

    let stopNote = frequency => {
        oscillators[frequency].stop(context.currentTime);
        oscillators[frequency].disconnect();
    };
    
    return {
        playNote: playNote,
        stopNote: stopNote
    };
}