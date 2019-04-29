public class PIDController {


    float kP, kI, kD;
    float error;
    float dInput;
    float P, I, D;
    public float setpoint;
    float lastInput = 8; // BAD HARDCODE
    float lastVal;
    float outMin, outMax;
    public PIDController(float kP, float kI, float kD, float outMin, float outMax) {
        this.kP = kP;
        this.kI = kI;
        this.kD = kD;
        this.outMin = outMin;
        this.outMax = outMax;
    }

    public void setSetpoint(float setpoint) {
        this.setpoint = setpoint;
    }

    public float update(float input, float dTime) {
        error = setpoint - input;
        dInput = input - lastInput;
        P = error * kP;
        I += error * kI * dTime;

        if(I > outMax) I = outMax;
        else if(I < outMin) I = outMin;

        D = -kD * dInput;
        lastInput = input;

        float output = P + I + D;
        if(output > outMax) output = outMax;
        else if(output < outMin) output = outMin;
        lastVal = output;
        return output;
    }

    public override string ToString() {
        //return "P: " + P + " I: " + I + " D: " + D;
        return "SP: " + setpoint + " IN: " + lastInput + " VAL: " + lastVal;
    }
}
