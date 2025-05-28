// Site JavaScript functionality
$(document).ready(function () {
    // Enable tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Enable popovers
    $('[data-toggle="popover"]').popover();

    // Appointment date picker
    if ($('#appointmentDate').length) {
        $('#appointmentDate').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true,
            todayHighlight: true,
            startDate: new Date()
        });
    }

    // Time slot selection
    if ($('#doctorId').length && $('#appointmentDate').length) {
        $('#doctorId, #appointmentDate').change(function () {
            var doctorId = $('#doctorId').val();
            var appointmentDate = $('#appointmentDate').val();
            
            if (doctorId && appointmentDate) {
                $.ajax({
                    url: '/Appointment/GetAvailableTimeSlots',
                    type: 'GET',
                    data: { doctorId: doctorId, date: appointmentDate },
                    success: function (timeSlots) {
                        var options = '<option value="">Zaman Seçiniz</option>';
                        $.each(timeSlots, function (i, slot) {
                            options += '<option value="' + slot + '">' + formatTime(slot) + '</option>';
                        });
                        $('#startTime').html(options);
                    },
                    error: function () {
                        alert('Zaman dilimlerini alırken bir hata oluştu.');
                    }
                });
            }
        });
    }

    // Format time for display
    function formatTime(timeString) {
        var parts = timeString.split(':');
        return parts[0] + ':' + parts[1];
    }

    // Confirm delete
    $('.delete-confirm').click(function (e) {
        if (!confirm('Bu kaydı silmek istediğinizden emin misiniz?')) {
            e.preventDefault();
        }
    });

    // Print functionality
    $('.print-button').click(function () {
        window.print();
    });

    // Search functionality
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $("#searchTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Dynamic form fields for prescription items
    let itemCount = 1;
    
    $('#addPrescriptionItem').click(function () {
        const newItem = `
            <div class="prescription-item border p-3 mb-3">
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>İlaç</label>
                        <select name="Items[${itemCount}].MedicationId" class="form-control" required>
                            <option value="">İlaç Seçiniz</option>
                            ${$('#medicationTemplate').html()}
                        </select>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Dozaj</label>
                        <input type="text" name="Items[${itemCount}].Dosage" class="form-control" required>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label>Kullanım Sıklığı</label>
                        <input type="text" name="Items[${itemCount}].Frequency" class="form-control" required>
                    </div>
                    <div class="form-group col-md-4">
                        <label>Kullanım Süresi</label>
                        <input type="text" name="Items[${itemCount}].Duration" class="form-control" required>
                    </div>
                    <div class="form-group col-md-4">
                        <label>Miktar</label>
                        <input type="number" name="Items[${itemCount}].Quantity" class="form-control" min="1" required>
                    </div>
                </div>
                <div class="form-group">
                    <label>Talimatlar</label>
                    <textarea name="Items[${itemCount}].Instructions" class="form-control" rows="2"></textarea>
                </div>
                <button type="button" class="btn btn-sm btn-danger remove-item">Kaldır</button>
            </div>
        `;
        
        $('#prescriptionItems').append(newItem);
        itemCount++;
    });
    
    // Remove prescription item
    $(document).on('click', '.remove-item', function () {
        $(this).closest('.prescription-item').remove();
    });

    // Dashboard charts
    if ($('#appointmentsChart').length) {
        var ctx = document.getElementById('appointmentsChart').getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi', 'Pazar'],
                datasets: [{
                    label: 'Haftalık Randevular',
                    data: [12, 19, 3, 5, 2, 3, 7],
                    backgroundColor: 'rgba(78, 115, 223, 0.2)',
                    borderColor: 'rgba(78, 115, 223, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    // Revenue chart
    if ($('#revenueChart').length) {
        var ctx = document.getElementById('revenueChart').getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran'],
                datasets: [{
                    label: 'Aylık Gelir (TL)',
                    data: [12500, 19000, 15000, 18000, 22000, 25000],
                    backgroundColor: [
                        'rgba(78, 115, 223, 0.2)',
                        'rgba(28, 200, 138, 0.2)',
                        'rgba(54, 185, 204, 0.2)',
                        'rgba(246, 194, 62, 0.2)',
                        'rgba(231, 74, 59, 0.2)',
                        'rgba(133, 135, 150, 0.2)'
                    ],
                    borderColor: [
                        'rgba(78, 115, 223, 1)',
                        'rgba(28, 200, 138, 1)',
                        'rgba(54, 185, 204, 1)',
                        'rgba(246, 194, 62, 1)',
                        'rgba(231, 74, 59, 1)',
                        'rgba(133, 135, 150, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
});
