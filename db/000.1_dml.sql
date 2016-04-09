insert into networktype
(entitytypeid, name, cost)
    VALUES
      (1, 'road corridor', 1),
      (2, 'sidewalk', 0.9)

insert into congestiontype
(congestiontypeid, name, cost)
    VALUES
      (1, 'construction', 1000),
      (2, 'emergency', 50),
      (3, 'major-event', 10)
