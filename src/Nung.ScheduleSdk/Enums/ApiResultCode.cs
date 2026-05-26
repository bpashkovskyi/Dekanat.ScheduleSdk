namespace Nung.ScheduleSdk.Enums;

/// <summary>
/// Коди результату з поля <c>code</c> у відповіді <c>psrozklad_export</c>.
/// </summary>
/// <remarks>
/// Сервер повертає <c>code</c> як рядок; для помилок у полі <c>error.errorcode</c> часто зберігається знакове значення
/// (наприклад <c>-90</c>), тоді як <c>code</c> може бути без знаку (<c>90</c>).
/// </remarks>
public enum ApiResultCode
{
    /// <summary>Успішне виконання запиту.</summary>
    Success = 0,

    /// <summary>Розклад у базі відсутній.</summary>
    ScheduleMissing = -1,

    /// <summary>Перегляд розкладу заблоковано адміністратором.</summary>
    ScheduleViewBlocked = -2,

    /// <summary>Перегляд розкладу на вказані дати заблоковано.</summary>
    ScheduleDateRangeBlocked = -3,

    /// <summary>За запитом нічого не знайдено.</summary>
    NotFound = -4,

    /// <summary>Технічні роботи на сервері.</summary>
    Maintenance = -5,

    /// <summary>Експорт всього розкладу закладу заборонено.</summary>
    FullExportForbidden = -6,

    /// <summary>Помилка у параметрах запиту.</summary>
    InvalidParameters = -60,

    /// <summary>Помилка у датах.</summary>
    InvalidDates = -70,

    /// <summary>Неправильний режим (<c>req_mode</c> або <c>req_type</c>).</summary>
    InvalidMode = -80,

    /// <summary>Об'єкт для розкладу не знайдено.</summary>
    ObjectNotFound = -90,

    /// <summary>Внутрішня помилка модуля експорту.</summary>
    ModuleError = -100,

    /// <summary>Немає доступу до сервера або помилка бази даних.</summary>
    DatabaseAccessError = -200,
}
