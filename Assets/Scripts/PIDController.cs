public class PIDController {


    float kP, kI, kD;
    float error;
    float dInput;
    float P, I, D;
    float setpoint;
    float lastInput = 8; // BAD HARDCODE
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
        return P+I+D;
    }

    public override string ToString() {
        return "P: " + P + " I: " + I + " D: " + D;
    }
}
