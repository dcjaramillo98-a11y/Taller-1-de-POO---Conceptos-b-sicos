namespace Taller01
{
    /// <summary>
    /// Representa una hora del día (00:00:00.000 a 23:59:59.999).
    /// </summary>
    public class Time
    {
        // ---------------------------------------------------------------
        // Campos
        // ---------------------------------------------------------------
        private int _hour;
        private int _minute;
        private int _second;
        private int _millisecond;

        // ---------------------------------------------------------------
        // Propiedades
        // ---------------------------------------------------------------
        public int Hour
        {
            get => _hour;
            set
            {
                if (!ValidHour(value))
                    throw new Exception($"The hour: {value}, is not valid.");
                _hour = value;
            }
        }

        public int Minute
        {
            get => _minute;
            set
            {
                if (!ValidMinute(value))
                    throw new Exception($"The minute: {value}, is not valid.");
                _minute = value;
            }
        }

        public int Second
        {
            get => _second;
            set
            {
                if (!ValidSecond(value))
                    throw new Exception($"The second: {value}, is not valid.");
                _second = value;
            }
        }

        public int Millisecond
        {
            get => _millisecond;
            set
            {
                if (!ValidMillisecond(value))
                    throw new Exception($"The millisecond: {value}, is not valid.");
                _millisecond = value;
            }
        }

        // ---------------------------------------------------------------
        // Constructores (5 sobrecargas — cualquier parámetro no indicado
        // se asume en cero)
        // ---------------------------------------------------------------
        public Time() : this(0, 0, 0, 0)
        {
        }

        public Time(int hour) : this(hour, 0, 0, 0)
        {
        }

        public Time(int hour, int minute) : this(hour, minute, 0, 0)
        {
        }

        public Time(int hour, int minute, int second) : this(hour, minute, second, 0)
        {
        }

        public Time(int hour, int minute, int second, int millisecond)
        {
            // Se asignan mediante las propiedades para que la validación
            // se dispare en el momento de construir el objeto.
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        // ---------------------------------------------------------------
        // Validaciones (privadas)
        // ---------------------------------------------------------------
        private bool ValidHour(int hour) => hour >= 0 && hour <= 23;

        private bool ValidMinute(int minute) => minute >= 0 && minute <= 59;

        private bool ValidSecond(int second) => second >= 0 && second <= 59;

        private bool ValidMillisecond(int millisecond) => millisecond >= 0 && millisecond <= 999;

        private bool IsValid() =>
            ValidHour(_hour) && ValidMinute(_minute) && ValidSecond(_second) && ValidMillisecond(_millisecond);

        // ---------------------------------------------------------------
        // Conversiones — devuelven 0 si la hora no es válida
        // ---------------------------------------------------------------
        public long ToMilliseconds()
        {
            if (!IsValid())
                return 0;

            return ((long)_hour * 3600 + _minute * 60 + _second) * 1000 + _millisecond;
        }

        public long ToSeconds()
        {
            if (!IsValid())
                return 0;

            return ToMilliseconds() / 1000;
        }

        public long ToMinutes()
        {
            if (!IsValid())
                return 0;

            return ToMilliseconds() / 60000;
        }

        // ---------------------------------------------------------------
        // IsOtherDay — indica si this + time cruza la medianoche
        // ---------------------------------------------------------------
        public bool IsOtherDay(Time time)
        {
            long totalMilliseconds = _millisecond + time._millisecond;
            long carrySeconds = totalMilliseconds / 1000;

            long totalSeconds = _second + time._second + carrySeconds;
            long carryMinutes = totalSeconds / 60;

            long totalMinutes = _minute + time._minute + carryMinutes;
            long carryHours = totalMinutes / 60;

            long totalHours = _hour + time._hour + carryHours;

            return totalHours >= 24;
        }

        // ---------------------------------------------------------------
        // Add — suma este Time con otro y retorna un nuevo Time,
        // llevando el acarreo de milisegundos -> segundos -> minutos ->
        // horas, y ajustando al día siguiente (módulo 24) si aplica.
        // ---------------------------------------------------------------
        public Time Add(Time time)
        {
            long totalMilliseconds = _millisecond + time._millisecond;
            int millisecond = (int)(totalMilliseconds % 1000);
            long carrySeconds = totalMilliseconds / 1000;

            long totalSeconds = _second + time._second + carrySeconds;
            int second = (int)(totalSeconds % 60);
            long carryMinutes = totalSeconds / 60;

            long totalMinutes = _minute + time._minute + carryMinutes;
            int minute = (int)(totalMinutes % 60);
            long carryHours = totalMinutes / 60;

            long totalHours = _hour + time._hour + carryHours;
            int hour = (int)(totalHours % 24);

            return new Time(hour, minute, second, millisecond);
        }

        // ---------------------------------------------------------------
        // ToString — formato HH:mm:ss.fff tt (NO militar)
        // ---------------------------------------------------------------
        public override string ToString()
        {
            if (!ValidHour(_hour))
                throw new Exception($"The hour: {_hour}, is not valid.");
            if (!ValidMinute(_minute))
                throw new Exception($"The minute: {_minute}, is not valid.");
            if (!ValidSecond(_second))
                throw new Exception($"The second: {_second}, is not valid.");
            if (!ValidMillisecond(_millisecond))
                throw new Exception($"The millisecond: {_millisecond}, is not valid.");

            string period = _hour < 12 ? "AM" : "PM";
            int hour12 = _hour % 12;

            return $"{hour12:00}:{_minute:00}:{_second:00}.{_millisecond:000} {period}";
        }
    }
}
