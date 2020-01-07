# AR-Mannequin-Projects
An augmented mannequin for medical education.

## Basic Desciption
This repo contains two Unity Projects.

**AR_Mannequin:** the main application which runs on the hololens.

**AR Mannequin - Teacher:** the teacher side application. User needs to initialize a room in this application for students/holoens application to join.

**Unity Version:** 2018.4.91f1(LTS)

This is copyed from previous github repo
## Base Overlay Functionality:

    body_assembly.blend contains all of the 3d models that we use for positioning of real life objects in the scene. The main models used for overlay are the body and head (these were 3d scanned and then retopologized for performance).

    precise positioning of the image targets is achieved through the image marker placement models (3d printed and installed on mannequin). Precise movement from IMU quaternion data is achieved through the IMUBoard (3d printed and installed) and IMUMount (represents the imu chip itself) model as well as using the neck-head joint model as a pivot point.

    IMUControl is the script that takes care of handshaking with the Arduino ble module and retrieving quaternion data
        the retrieved quaternion data is not related to the Unity scene (seperate coordinate system) so before any overlay can be done there must be a calibration step
            firstly, the axes are flipped, when creating the quaternion that represents the IMU's rotation, use Quaternion calibration = new Quaternion(-rotZ, -rotY, rotX, rotW)
            rotX, rotY, rotZ, and rotW are number values extracted from the data that is passed through the bluetooth device.
            when ready to calibrate, create the quaternion mentioned above, use calibration = Quaternion.Inverse(Quaternion.RotateTowards(pivot.transform.rotation, calibration, 180f)) where pivot is the GameObject the that represents the default position and orientation of the pivot point in the unity scene. Using this transformation, we know how to get from the first quaternion received to the target model's (the model that we want to move around) default rotation.
            in subsequent quaternion updates from the imu device, we can use this calibration quaternion like so quaternion currentMatrix = new Quaternion(-rotZ, -rotY, rotX, rotW); go.transform.rotation = calibration * currentMatrix

    We have 4 vuforia image targets the positioning of which is defined by the 3d modeled and 3d printed image marker structure.
