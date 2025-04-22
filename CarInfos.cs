using UnityEngine;

namespace CarsExtended
{
    /// <summary>Holds all information needed to inject cars</summary>
    public class CarInfos
    {
        public enum EngineOrientation
        {
            Transverse,
            Longitudinal
        }

        public enum SourceCar
        {
            // Group 2 (60's)
            the_esky_v1,
            the_meanie,
            la_montaine,
            das_220,
            das_119i,
            le_gorde,
            la_regina,
            the_rotary_kei,

            // Group 3 (70's)
            the_esky_v2,
            il_nonno_313,
            the_rotary_3,
            la_wedge,
            il_cavallo_803,
            das_119e,
            la_hepta,
            the_pebble_v1,
            the_pebble_v2,
            the_zetto,

            // Group 4 (80's)
            le_cinq,
            das_whip,
            turbo_brick,
            das_uberwhip,
            the_cozzie_sr5,
            the_original,
            la_super_montaine,
            la_longana,
            the_gazelle,
            das_scholar,

            // Group B
            the_4r6,
            le_502,
            das_hammer_v1,
            das_hammer_v2,
            il_gorilla_4s,
            the_cozzie_sr2,
            the_cozzie_sr71,
            the_rotary_b7,
            das_hammer_v3,
            il_monster,
            il_cavallo_882,
            le_cinq_b,
            das_uberspeedvan,
            the_king_of_africa,
            das_559,
            the_hyena,
            das_maestro,

            // Group S
            das_eibenhammer,
            the_rotary_s7,
            the_umibozu,
            il_gorilla_e1,
            le_504,
            il_gorilla_e2,
            das_superbaus,
            the_t22,

            // Group A
            il_gorillona,
            the_fujin,
            the_liftback,
            the_max_attack,
            the_cozzie_90,
            the_kingpin,

            // Vans
            das_speedvan,
            das_hi_speedvan,
            das_cube_van,
            funselektors_van,

            // Piaggio
            little_monkey,

            // Logging
            log_transporter,

            // Dakar
            dakar_truck
        }

        public Car data;

        public Vector3 engineOrientation; // not sure I really need this...
        public float wheelWidth = 0.26f;
        public float wheelRadius = 0.34f;
        public string soundSourcePrefabName;

        // TODO : Should I make a guide about settings ? (sizes examples and how it touches behaviours)
        // Test car for each group
        // Average settings for the class
        // Test grey model
        // Description is min/max for each setting + example of car for min/max setting

        public CarInfos(Car car, EngineOrientation engineOrientation, float wheelWidth, float wheelRadius, SourceCar soundSource)
        {
            data = car;

            if (engineOrientation == EngineOrientation.Longitudinal)
                this.engineOrientation = Vector3.forward;
            else if (engineOrientation == EngineOrientation.Transverse)
                this.engineOrientation = Vector3.right;

            this.wheelWidth = wheelWidth;
            this.wheelRadius = wheelRadius;
            soundSourcePrefabName = GetCarPrefabName(soundSource);
        }

        private string GetCarPrefabName(SourceCar sourceCar)
        {
            string sourceName = sourceCar.ToString().Replace("_", " ");

            switch (sourceCar)
            {
                case SourceCar.funselektors_van:
                    sourceName = "funselektor's van";
                    break;

                case SourceCar.das_hi_speedvan:
                    sourceName = "das hi-speedvan";
                    break;
            }

            Car selected = CarManager.AllCarsList.Find(item => item.name == sourceName);

            if (selected == null)
            {
                Main.Error("Couldn't find car for source \"" + sourceCar + "\"");
                return "ERROR";
            }

            return selected.prefabName;
        }
    }
}
